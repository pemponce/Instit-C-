using System;
using System.Collections.Generic;

namespace CustomStackExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("|-------------------|");
            Console.WriteLine("|Введите начало интервала:|");
            Console.WriteLine("|-------------------|");
            int startInterval = int.Parse(Console.ReadLine());

            Console.WriteLine("|-------------------|");
            Console.WriteLine("|Введите конец интервала:|");
            Console.WriteLine("|-------------------|");
            int endInterval = int.Parse(Console.ReadLine());

            Console.WriteLine("|-----------------------------|");
            Console.WriteLine("|Введите числа (через пробел):|");
            Console.WriteLine("|-----------------------------|");
            string input = Console.ReadLine();

            string[] numbersArray = input.Split(' ');
            CustomStack<int> numberStack = new CustomStack<int>();

            foreach (var numberStr in numbersArray)
            {
                if (int.TryParse(numberStr, out int number))
                {
                    numberStack.Push(number);
                }
            }

            List<int> leftOfRange = new List<int>();
            List<int> withinRange = new List<int>();
            List<int> rightOfRange = new List<int>();

            while (!numberStack.IsEmpty)
            {
                int number = numberStack.Pop();

                if (number < startInterval)
                {
                    leftOfRange.Add(number);
                }
                else if (number >= startInterval && number <= endInterval)
                {
                    withinRange.Add(number);
                }
                else
                {
                    rightOfRange.Add(number);
                }
            }

            Console.WriteLine("|Числа слева от интервала:|" + string.Join(", ", leftOfRange));
            Console.WriteLine("|Числа внутри интервала:|" + string.Join(", ", withinRange));
            Console.WriteLine("|Числа справа от интервала:|" + string.Join(", ", rightOfRange));
        }
    }

    class CustomStack<T>
    {
        private StackItem<T> top;

        public bool IsEmpty => top == null;

        public void Push(T item)
        {
            StackItem<T> newItem = new StackItem<T>(item);
            newItem.Next = top;
            top = newItem;
        }

        public T Peek()
        {
            return top.Value;
        }

        public T Pop()
        {
            T poppedItem = top.Value;
            top = top.Next;
            return poppedItem;
        }
    }

    class StackItem<T>
    {
        public T Value { get; }
        public StackItem<T> Next { get; set; }

        public StackItem(T value)
        {
            Value = value;
            Next = null;
        }
    }
}
