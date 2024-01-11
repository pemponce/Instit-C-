// See https://aka.ms/new-console-template for more information


// Thread t1 = new Thread(Print);
// Thread t2 = new Thread(() => Console.WriteLine("hello"));
// t1.Start();
// t2.Start();
//
// void Print()
// {
//     for (int i = 0; i < 5; i++)
//     {
//         Console.WriteLine($"thread{i}");
//         Thread.Sleep(400);
//     }
// }
////////////////////////////////////////////////////////////////////////////////////////////////////
// int n = 4;
//
// Thread t3 = new Thread(Print);
// t3.Start(n);
//
// void Print(object o) {
//     if (o is int x)
//     {
//         Console.WriteLine($"{x*x}");
//     }
// }
////////////////////////////////////////////////////////////////////////////////////////////////////
// Person tom = new Person("Tom", 20);
// Thread t4 = new Thread(tom.Print);
// t4.Start();
//
// record class Person(string Name, int Age)
// {
//     public void Print()
//     {
//         Console.WriteLine($"{Name}, {Age}");
//     }
// }
////////////////////////////////////////////////////////////////////////////////////////////////////
// int x = 0;
// for (int i = 0; i < 6; i++)
// {
//     Thread t = new Thread(Print);
//     t.Name = $"thread {i}";
//     t.Start();
// }
//
// void Print()
// {
//     for (int i = 0; i < 5; i++)
//     {
//         Console.WriteLine($"{Thread.CurrentThread.Name}:{x}");
//         x++;
//         Thread.Sleep(300);
//     }
// }
//////////////////////////////////////////////////////////////////////////
// object loker = new object();
// int x = 0;
// for (int i = 0; i < 6; i++)
// {
//     Thread t = new Thread(Print);
//     t.Name = $"thread {i}";
//     t.Start();
// }
//
// void Print()
// {
//     lock (loker)
//     {
//         for (int i = 0; i < 5; i++)
//         {
//             Console.WriteLine($"{Thread.CurrentThread.Name}:{x}");
//             x++;
//             Thread.Sleep(300);
//         }
//     }
// }
///////////////////////////////////////////////////////////////////////////////////
object loker = new object();
int x = 0;
for (int i = 0; i < 6; i++)
{
    Thread t = new Thread(Print);
    t.Name = $"thread {i}";
    t.Start();
}
void Print()
{
    bool f = false;
    try
    {
        Monitor.Enter(loker, ref f);
        x = 1;
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name}:{x}");
            x++;
            Thread.Sleep(300);
        }
    }
    finally
    {
        if (f)
        {
            Monitor.Exit(loker);
        }
    }
}