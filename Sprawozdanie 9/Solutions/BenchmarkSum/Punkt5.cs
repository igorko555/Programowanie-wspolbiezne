using System;
using System.Diagnostics;

namespace BenchmarkSum
{
    class Punkt5
    {
        static int[] GenerateRandomArray(int size)
        {
            Random random = new Random();
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(10);
            }
            return array;
        }

        static void Main(string[] args)
        {
            const int size = 10000000;
            Console.WriteLine($"Zadanie 5: Sumowanie jednowatkowe tablicy {size} liczb\n");

            Stopwatch sw = Stopwatch.StartNew();
            int[] numbers = GenerateRandomArray(size);
            sw.Stop();
            Console.WriteLine($"Generowanie tablicy: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            long sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            sw.Stop();

            Console.WriteLine($"Sekwencyjny foreach | Suma: {sum} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
