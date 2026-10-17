using System;

namespace SimpleDelegate
{
    public delegate void MathOperation(int a, int b);

    class Punkt1
    {
        static void PrintSum(int a, int b)
        {
            Console.WriteLine($"{a} + {b} = {a + b}");
        }

        static void PrintDifference(int a, int b)
        {
            Console.WriteLine($"{a} - {b} = {a - b}");
        }

        static void PrintProduct(int a, int b)
        {
            Console.WriteLine($"{a} * {b} = {a * b}");
        }

        static void PrintQuotient(int a, int b)
        {
            if (b != 0)
            {
                Console.WriteLine($"{a} / {b} = {(double)a / b:F2}");
            }
            else
            {
                Console.WriteLine("Dzielenie przez zero!");
            }
        }

        static void Main(string[] args)
        {
            int x = 20;
            int y = 4;

            MathOperation mathOp = new MathOperation(PrintSum);
            mathOp += PrintDifference;
            mathOp += PrintProduct;
            mathOp += PrintQuotient;

            mathOp(x, y);

            Console.WriteLine("\nNazwy metod z listy wywolan:");
            foreach (var item in mathOp.GetInvocationList())
            {
                Console.WriteLine(item.Method.Name);
            }
        }
    }
}
