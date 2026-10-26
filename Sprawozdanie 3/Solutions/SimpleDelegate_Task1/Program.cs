using System;

namespace SimpleDelegate_Task1
{
    class Program
    {
        // --------------------------------------------------------------------------------------------------
        // ДЕЛЕГАТ: Визначає сигнатуру методу, який повертає void і приймає два цілочисельні параметри (int, int).
        // Делегат у C# є типобезпечним об'єктно-орієнтованим покажчиком на методи (клас-нащадок MulticastDelegate).
        // --------------------------------------------------------------------------------------------------
        public delegate void MathOperation(int a, int b);

        // 1. Метод обчислення та виведення суми
        static void PrintSum(int a, int b)
        {
            Console.WriteLine($"[PrintSum]        {a} + {b} = {a + b}");
        }

        // 2. Метод обчислення та виведення різниці
        static void PrintDifference(int a, int b)
        {
            Console.WriteLine($"[PrintDifference] {a} - {b} = {a - b}");
        }

        // 3. Метод обчислення та виведення добутку (iloczyn)
        static void PrintProduct(int a, int b)
        {
            Console.WriteLine($"[PrintProduct]    {a} * {b} = {a * b}");
        }

        // 4. Метод обчислення та виведення частки (iloraz) із захистом від ділення на нуль
        static void PrintQuotient(int a, int b)
        {
            if (b == 0)
            {
                Console.WriteLine($"[PrintQuotient]   {a} / {b} => БЛОКУВАННЯ: Ділення на нуль неможливе!");
            }
            else
            {
                double result = (double)a / b;
                Console.WriteLine($"[PrintQuotient]   {a} / {b} = {result:F2}");
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================================");
            Console.WriteLine("   LABORATORIUM 3 - ZADANIE 1: PROSTY DELEGAT ARYTMETYCZNY (C#)   ");
            Console.WriteLine("==================================================================\n");

            int x = 20;
            int y = 4;
            Console.WriteLine($"Аргументи для обчислень: x = {x}, y = {y}\n");

            // Ініціалізація екземпляра делегата першим цільовим методом
            MathOperation mathOp = new MathOperation(PrintSum);

            // Додавання наступних методів до ланцюга викликів (Multicast Delegate) за допомогою оператора +=
            mathOp += PrintDifference;
            mathOp += PrintProduct;
            mathOp += PrintQuotient;

            // 1. Виклик делегата (послідовне виконання всіх методів з черги викликів)
            Console.WriteLine("--- 1. ВИКЛИК ЛАНЦЮГА ДЕЛЕГАТА (MULTICAST INVOCATION) ---");
            mathOp(x, y);

            // 2. Інспекція списку викликів делегата за допомогою GetInvocationList()
            Console.WriteLine("\n--- 2. МЕТОДИ В СПИСКУ ВИКЛИКІВ ДЕЛЕГАТА (GetInvocationList) ---");
            Delegate[] invocationList = mathOp.GetInvocationList();
            for (int i = 0; i < invocationList.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] Назва методу: {invocationList[i].Method.Name}, Клас: {invocationList[i].Method.DeclaringType?.Name}");
            }

            Console.WriteLine("\n==================================================================");
            Console.WriteLine("Завдання 1 успішно виконано.");
        }
    }
}
