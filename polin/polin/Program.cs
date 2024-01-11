﻿namespace polin;

internal class Programm
{
    public static void Main(string[] args)
    {
        Parse_Test();
        Solve_Test();
        Normalize_Test();
        Add_Test();
        Subtract_Test();
        Multiply_Test();
        MultiplyByValue_Test();
        DivideByValue_Test();
        Equals_Test();

        Console.WriteLine("Tests passed!");

        var preparedPolynoms = new[]
        {
            Polynom.Parse("x^(2)+1"),
            Polynom.Parse("x^(2)+10"),
            Polynom.Parse("x^(2)+6"),
            Polynom.Parse("x^(2)+3"),
            Polynom.Parse("x^(2)+2"),
            Polynom.Parse("x^(2)+12")
        };

        SortByPoint(preparedPolynoms, 2);

        foreach (var polynom in preparedPolynoms)
            Console.WriteLine(polynom);

        var polynoms = new List<Polynom>();
        var count = int.Parse(Console.ReadLine());

        for (var i = 0; i < count; i++)
            polynoms.Add(Polynom.Parse(Console.ReadLine()));

        var x = double.Parse(Console.ReadLine());

        SortByPoint(preparedPolynoms, x);

        foreach (var polynom in preparedPolynoms)
            Console.WriteLine(polynom);
    }

    private static void SortByPoint(Polynom[] polynoms, double x)
    {
        var dict = new Dictionary<Polynom, double>();

        foreach (var polynom in polynoms) dict[polynom] = polynom.Solve(x);
        var ordered = dict.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value).Keys
            .ToArray();

        for (var i = 0; i < ordered.Length; i++)
            polynoms[i] = ordered[i];
    }

    private static void Parse_Test()
    {
        var polynom = Polynom.Parse("4x^(3)+2x^(4)+4x^(3)+3x^(4)");
        const string expected = "5x^(4)+8x^(3)";
        var result = polynom.ToString();

        Assert(result != expected);
    }

    private static void Solve_Test()
    {
        var polynom = Polynom.Parse("2x^(2)-3x+5");

        const double x = 2;
        const double expected = 7;
        var result = polynom.Solve(x);

        Assert(result != expected);
    }

    private static void Normalize_Test()
    {
        var polynom = Polynom.Parse("3x^(2)+4x^(3)-2x^(2)-5x^(4)");
        const string expected = "-5x^(4)+4x^(3)+x^(2)";
        var result = polynom.ToString();
        Assert(result != expected);
    }

    private static void Add_Test()
    {
        var polynom1 = Polynom.Parse("2x^(2)+3x+5");
        var polynom2 = Polynom.Parse("3x^(2)-2x+1");
        var result = polynom1 + polynom2;
        const string expected = "5x^(2)+x+6";
        var strResult = result.ToString();

        Assert(strResult != expected);
    }

    private static void Subtract_Test()
    {
        var polynom1 = Polynom.Parse("2x^(2)+3x+5");
        var polynom2 = Polynom.Parse("3x^(2)-2x+1");
        var result = polynom1 - polynom2;
        const string expected = "-x^(2)+5x+4";
        var strResult = result.ToString();
        Assert(strResult != expected);
    }

    private static void Multiply_Test()
    {
        var polynom1 = Polynom.Parse("2x^(2)+3x+5");
        var polynom2 = Polynom.Parse("3x^(2)-2x+1");
        var result = polynom1 * polynom2;
        const string expected = "6x^(4)+5x^(3)+11x^(2)-7x+5";
        var strResult = result.ToString();
        Assert(strResult != expected);
    }

    private static void MultiplyByValue_Test()
    {
        var polynom = Polynom.Parse("2x^(2)+3x+5");
        const double value = 2;
        var result = polynom * value;
        const string expected = "4x^(2)+6x+10";
        var strResult = result.ToString();
        Assert(strResult != expected);
    }

    private static void DivideByValue_Test()
    {
        var polynom = Polynom.Parse("2x^(2) + 3x + 5");
        const double value = 2;
        var result = polynom / value;
        const string expected = "x^(2)+1.5x+2.5";
        var strResult = result.ToString();
        Assert(strResult != expected);
    }

    private static void Equals_Test()
    {
        var polynom1 = Polynom.Parse("2x^(2)+3x+5");
        var polynom2 = Polynom.Parse("2x^(2)+3x+5");
        var polynom3 = Polynom.Parse("x^(2)+1.5x+2.5");

        Assert(!(polynom1 == polynom2));
        Assert(!(polynom1 != polynom3));
    }

    private static void Assert(bool b)
    {
        if (b) throw new Exception();
    }
}

public class Polynom
{
    private List<Uninomial> _uninomials;

    private Polynom()
    {
        _uninomials = new List<Uninomial>();
    }

    private Polynom(List<Uninomial> uninomials)
    {
        _uninomials = uninomials;
        Normalize();
    }

    public static Polynom Parse(string input)
    {
        var uninomials = new List<Uninomial>();
        var uninomial = "";

        input = input.Replace(" ", "");

        foreach (var chapter in input)
        {
            if (!"+-".Contains(chapter))
                uninomial += chapter;

            else if (chapter == '-' && uninomial.Length == 0)
                uninomial = "-";

            else
            {
                uninomials.Add(Uninomial.Parse(uninomial));
                uninomial = chapter + "";
            }
        }

        uninomials.Add(Uninomial.Parse(uninomial));
        return new Polynom(uninomials);
    }

    public double Solve(double x)
    {
        return _uninomials.Sum(uninomial => uninomial.Solve(x));
    }

    public void Normalize()
    {
        var powers = new Dictionary<int, Uninomial>();

        foreach (var uninomial in _uninomials)
        {
            var power = uninomial.Power;

            if (powers.TryGetValue(power, out var old)) powers[power] = uninomial + old;
            else powers.Add(power, uninomial);
        }

        _uninomials = powers.Values
            .Where(uninomial => uninomial.Multiplier != 0)
            .OrderBy(uninomial => uninomial.Power)
            .Reverse()
            .ToList();
    }

    public static Polynom operator +(Polynom first, Polynom second)
    {
        return Operate(first, second, (a, b) => a + b);
    }

    public static Polynom operator -(Polynom first, Polynom second)
    {
        return Operate(first, second, (a, b) => a - b);
    }

    public static Polynom operator *(Polynom first, Polynom second)
    {
        var result = new Polynom();

        first.Normalize();
        second.Normalize();

        var powers = new Dictionary<int, Uninomial>();

        foreach (var a in first._uninomials)
        foreach (var b in second._uninomials)
        {
            var uninomial = new Uninomial(a.Multiplier * b.Multiplier, a.Power + b.Power);
            var power = uninomial.Power;

            if (powers.TryGetValue(power, out var old)) powers[power] = uninomial + old;
            else powers.Add(power, uninomial);
        }

        result._uninomials = new List<Uninomial>(powers.Values);
        result.Normalize();

        return result;
    }

    public static Polynom operator *(Polynom polynom, double value)
    {
        var result = new Polynom();
        polynom.Normalize();

        result._uninomials = polynom._uninomials
            .Select(uninomial => uninomial * value)
            .ToList();

        return result;
    }

    public static Polynom operator /(Polynom polynom, double value)
    {
        var result = new Polynom();
        polynom.Normalize();

        result._uninomials = polynom._uninomials
            .Select(uninomial => uninomial / value)
            .ToList();

        return result;
    }

    public static bool operator ==(Polynom first, Polynom second)
    {
        first.Normalize();
        second.Normalize();

        if (first._uninomials.Count != second._uninomials.Count) return false;

        return !first._uninomials.Where((t, i) => t != second._uninomials[i]).Any();
    }

    public static bool operator !=(Polynom first, Polynom second)
    {
        return !(first == second);
    }

    public static Polynom Operate(Polynom first, Polynom second, Func<Uninomial, Uninomial, Uninomial> operation)
    {
        var result = new Polynom();

        first.Normalize();
        second.Normalize();

        var powers = new Dictionary<int, Uninomial>();

        var uninomials = first._uninomials.ToList();
        uninomials.AddRange(second._uninomials);

        foreach (var uninomial in uninomials)
        {
            var power = uninomial.Power;

            if (powers.TryGetValue(power, out var old)) powers[power] = operation.Invoke(old, uninomial);
            else powers.Add(power, uninomial);
        }

        result._uninomials = new List<Uninomial>(powers.Values);
        return result;
    }

    public override string ToString()
    {
        var result = "";
        if (_uninomials.Count == 0) return result;

        result += _uninomials[0];
        if (result.StartsWith("+")) result = result[1..];

        for (var i = 1; i < _uninomials.Count; i++) result += _uninomials[i];
        return result;
    }
}

public class Uninomial
{
    public Uninomial(double multiplier, int power)
    {
        Multiplier = multiplier;
        Power = power;
    }

    public double Multiplier { set; get; }
    public int Power { set; get; }

    public static Uninomial Parse(string input)
    {
        input = input.Replace(".", ",");
        string mul = "1", pow = "0";
        const char v = 'x';

        if (input == "x")
        {
            mul = "1";
        }
        else if (!input.Contains(v))
        {
            mul = input;
        }
        else if (input.EndsWith(v))
        {
            mul = input[..^1];
            pow = "1";
        }
        else if (input.StartsWith(v))
        {
            mul = "1";
            pow = input[3..^1];
        }
        else
        {
            var row = input.Split(v);
            mul = row[0];
            pow = row[1][2..^1];
        }

        return new Uninomial(double.Parse(mul), int.Parse(pow));
    }

    public static Uninomial operator +(Uninomial first, Uninomial second)
    {
        if (first.Power != second.Power) throw new ArgumentException("first and second args has differetn powers");
        return new Uninomial(first.Multiplier + second.Multiplier, first.Power);
    }

    public static Uninomial operator -(Uninomial first, Uninomial second)
    {
        if (first.Power != second.Power) throw new ArgumentException("first and second args has differetn powers");
        return new Uninomial(first.Multiplier - second.Multiplier, first.Power);
    }

    public static Uninomial operator *(Uninomial uninomial, double value)
    {
        return new Uninomial(uninomial.Multiplier * value, uninomial.Power);
    }

    public static Uninomial operator /(Uninomial uninomial, double value)
    {
        return new Uninomial(uninomial.Multiplier / value, uninomial.Power);
    }

    public static bool operator ==(Uninomial first, Uninomial second)
    {
        return first.Multiplier == second.Multiplier && first.Power == second.Power;
    }

    public static bool operator !=(Uninomial first, Uninomial second)
    {
        return !(first == second);
    }

    public double Solve(double x)
    {
        return Multiplier * Math.Pow(x, Power);
    }

    public override string ToString()
    {
        var mul = Math.Abs(Multiplier).ToString().Replace(",", ".");
        var sign = Multiplier > 0 ? "+" : "-";

        if (Power == 0) return sign + mul;
        if (mul == "1" && Power == 1) return sign + "x";
        if (mul == "1" && Power != 1) return sign + "x^(" + Power + ")";
        if (Power == 1) return sign + mul + "x";
        return sign + mul + "x^(" + Power + ")";
    }
}