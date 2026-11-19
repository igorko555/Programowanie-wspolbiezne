using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt7
    {
        static int[] GenerateRandomArray(int size)
        {
            Random random = new Random(42);
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
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"Zadanie 7: Sumowanie partycjami Task na {cores} rdzeniach ({size} liczb)\n");

            int[] numbers = GenerateRandomArray(size);
            int chunkSize = size / cores;
            Task<double>[] tasks = new Task<double>[cores];

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < cores; i++)
            {
                int start = i * chunkSize;
                int end = (i == cores - 1) ? size : start + chunkSize;
                tasks[i] = Task.Run(() =>
                {
                    double localSum = 0;
                    for (int j = start; j < end; j++)
                    {
                        localSum += numbers[j];
                    }
                    return localSum;
                });
            }

            double[] partialResults = Task.WhenAll(tasks).Result;
            double sum = partialResults.Sum();
            sw.Stop();

            Console.WriteLine($"Partycje Task.Run | Suma = {sum:F0} | Czas = {sw.ElapsedMilliseconds} ms");
        }
    }
}
