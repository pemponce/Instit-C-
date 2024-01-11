using System;
using System.Net;

namespace cat;

class Program
{
    // Инициализируем ивенты.
    static event Action CatFoundCookieEvent;
    static event Action CatStarvedEvent;
    static event Action CatOverfedEvent;

    // Из класса Кэт берем и иницализируем Энергию кота и его изначальную позицию.
    static int catEnergy = Cat.catEnergy;
    static int[] catPosition = Cat.catPosition;
    
    // Начальная позиция 1 и 2 печеньки
    static int[] cookiePosition = Cookies.cookiePosition;
    static int[] cookiePosition1 = Cookies.cookiePosition1;

    // инициализируем строки и столбцы нашего поля
    static int rows = Board.rows;
    static int cols = Board.cols;

    static void Main()
    {
        // Ивент в случае когда кот нашел еду
        CatFoundCookieEvent += () =>
        {
            // прибавляем к энергии кота магическое число
            catEnergy += ((rows * cols) / 10) + 10;
            // вызываем метод спавнищий печеньку
            PlaceCookie();
        };

        // ивент в случае если энергия кота < 1, кот недоедает и умирает
        CatStarvedEvent += () => Console.WriteLine("Kitty starved to death");
        // ивент в случае если энергия кота > 200, кот переедает и умирает
        CatOverfedEvent += () => Console.WriteLine("Died of gluttony");

        InitializeGame();

        while (catEnergy > 0 && catEnergy < 200)
        {
            PrintBoard();
            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();

            MoveCat(key);

            if (catPosition[0] == cookiePosition[0] && catPosition[1] == cookiePosition[1] || catPosition[0] == cookiePosition1[0] && catPosition[1] == cookiePosition1[1])
            {
                CatFoundCookieEvent?.Invoke();
            }

            catEnergy -= (rows * cols) / 10;

            if (catEnergy <= 0)
            {
                CatStarvedEvent?.Invoke();
            }
            else if (catEnergy >= 200)
            {
                CatOverfedEvent?.Invoke();
            }
        }
    }

    //начало
    static void InitializeGame()
    {
        // спавним котика
        PlaceCat();
        // спавним печеньки
        InitCookie();
    }

    // метод который спавнит на рандомных координатах, две печеньки
    private static void InitCookie()
    {
        cookiePosition[0] = new Random().Next(rows);
        cookiePosition[1] = new Random().Next(cols);
    }

    // метод который спавнит на рандомной координате, котика
    static void PlaceCat()
    {
        catPosition[0] = new Random().Next(rows);
        catPosition[1] = new Random().Next(cols);
    }

    // метод который вызывается в случае если котик нашел печеньку, и она должна переспавниться
    static void PlaceCookie()
    {
        // переспавн первой печеньки если координаты кота и печеньки совпадают (кот сьел печеньку)
        if (catPosition[0] == cookiePosition[0] && catPosition[1] == cookiePosition[1])
        {
            cookiePosition[0] = new Random().Next(rows);
            cookiePosition[1] = new Random().Next(cols);
            
            // предотвращаем появление печеньки на месте другого обьекта на игровом поле
            while (cookiePosition[0] == cookiePosition1[0] && cookiePosition[1] == cookiePosition1[1])
            {
                cookiePosition[0] = new Random().Next(rows);
                cookiePosition[1] = new Random().Next(cols);
            }
        }
        // переспавн второй печеньки если координаты кота и печеньки совпадают (кот сьел печеньку)
        else if (catPosition[0] == cookiePosition1[0] && catPosition[1] == cookiePosition1[1])
        {
            cookiePosition1[0] = new Random().Next(rows);
            cookiePosition1[1] = new Random().Next(cols);
            // предотвращаем появление печеньки на месте другого обьекта на игровом поле
            while (cookiePosition[0] == cookiePosition1[0] && cookiePosition[1] == cookiePosition1[1])
            {
                cookiePosition1[0] = new Random().Next(rows);
                cookiePosition1[1] = new Random().Next(cols);
            }
        }
    }
    
    // передвижение котика на клавиши вверх вниз влево и вправо
    static void MoveCat(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (catPosition[0] > 0)
                    catPosition[0]--;
                break;
            case ConsoleKey.DownArrow:
                if (catPosition[0] < rows - 1)
                    catPosition[0]++;
                break;
            case ConsoleKey.LeftArrow:
                if (catPosition[1] > 0)
                    catPosition[1]--;
                break;
            case ConsoleKey.RightArrow:
                if (catPosition[1] < cols - 1)
                    catPosition[1]++;
                break;
        }
    }

    // отрисовка игрового поля
    static void PrintBoard()
    {
        Console.Clear();
        if (catEnergy > 0 && catEnergy < 200)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == catPosition[0] && j == catPosition[1])
                        Console.Write("0"); // Позиция кошки
                    else if (i == cookiePosition[0] && j == cookiePosition[1])
                        Console.Write("*"); // Позиция печеньки
                    else if (i == cookiePosition1[0] && j == cookiePosition1[1])
                        Console.Write("*"); // Позиция печеньки
                    else
                        Console.Write("#"); // Пустая клетка
                }
                Console.WriteLine();
            }
        }
        
        // счетчик энергии котика
        Console.WriteLine($"Cat Energy: {catEnergy}");
    }
}
