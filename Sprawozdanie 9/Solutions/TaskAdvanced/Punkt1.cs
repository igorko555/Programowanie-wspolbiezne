using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAdvanced
{
    class Punkt1
    {
        static Task<int[]> calculateSomethingAsync(int size)
        {
            return Task.Factory.StartNew((s) =>
            {
                if (s is null) throw new ArgumentNullException(nameof(s));
                int[] result = new int[(int)s];
                for (int i = 0; i < (int)s; i++)
                {
                    Thread.SpinWait(10000000);
                    result[i] = Random.Shared.Next(10);
                    Console.WriteLine($"I calculated element {i} as [{result[i]}]");
                }
                return result;
            }, size);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Zadanie 1: Podstawowa metoda calculateSomethingAsync\n");

            Task<int[]> task = calculateSomethingAsync(10);
            task.Wait();

            Console.WriteLine($"\nObliczenia zakonczone. Liczba wygenerowanych elementow: {task.Result.Length}");
        }
    }
}
