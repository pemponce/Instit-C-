using System;

namespace Program
{
    class Program
    {
        public delegate double Fun(double x);

        public delegate double Met(Fun fun, double a, double b, double e);

        // 1) x^3 - 20x - 2
        static double Fun1(double x)
        {
            return x * x * x - 20 * x - 2;
        }

        static double Fun2(double x)
        {
            return x * x + x - 12;
        }

        public static double XordMethod(Fun fun, double a, double b, double e)
        {
            Console.WriteLine("Метод хорд");
            int i = 0;
            double x = 0; // приближение к корню
            double t;
            do
            {
                t = x;
                x = b - fun(b) * (b - a) / (fun(b) - fun(a));

                // в какой половине находится корень
                if (fun(x) * fun(a) > 0)
                {
                    a = x;
                } // [x; b]
                else
                {
                    b = x;
                } //[a; x]

                i++;
            } while (Math.Abs(x - t) > e || (b - a) <= e);

            Console.WriteLine($"Кол-во итераций: {i}");
            return x;
        }

        public static double KasatelMethod(Fun fun, double a, double b, double e)
        {
            double x0 = (a + b) / 2;
            int i = 0;
            while (Math.Abs(fun(x0)) > e)
            {
                i++;
                double proiz = (fun(x0 + e) - fun(x0)) / e; // по конечной разности
                // обновляем приближени по методу ньютона
                x0 = x0 - fun(x0) / proiz;
            }

            Console.WriteLine($"Кол-во итераций: {i}");
            return x0;
        }

        public static double DelenMethod(Fun fun, double a, double b, double e)
        {
            Console.WriteLine("Метод деления");

            int i = 0;
            while (true)
            {
                double seredina = (a + b) / 2;
                i++;

                if (fun(seredina) == 0.0 || Math.Abs(b - a) < Math.Abs(e))
                {
                    Console.WriteLine($"Кол-во итераций: {i}");
                    return seredina;
                }

                // в зависимости от знака 
                if ((fun(seredina) > 0 && fun(a) > 0) || (fun(seredina) < 0 && fun(a) < 0))
                {
                    a = seredina;
                } // [seredina; b]

                else
                {
                    b = seredina;
                } // [a; seredina]
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Выберите функцию:");
            Console.WriteLine("1) x^3 - 20x - 2    2) x^2+x-12");


            // экземпляр делегата
            Fun fun = null;


            int num = Convert.ToInt32(Console.ReadLine());
            if (num == 1)
            {
                fun = Fun1;
            }

            if (num == 2)
            {
                fun = Fun2;
            }
            
            Console.WriteLine();


            Console.WriteLine("Выберите метод");
            Console.WriteLine("1 -- метод хорд   2 -- метод касательной   3 -- метод деления отрезков");
            Met method = null;
            int num1 = Convert.ToInt32(Console.ReadLine());
            if (num1 == 1)
            {
                method = XordMethod;
            }

            if (num1 == 2)
            {
                method = KasatelMethod;
            }

            if (num1 == 3)
            {
                method = DelenMethod;
            }

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Введите отрезок на котором ищете функцию");
                double a = Convert.ToDouble(Console.ReadLine());
                double b = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine();

                Console.WriteLine("Введите точность через ,"); 
                double e = Convert.ToDouble(Console.ReadLine());

                try
                {
                    Console.WriteLine(method(fun, a, b, e));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.WriteLine("Выберите другой отрезок");
                }
            }
        }
    }
}