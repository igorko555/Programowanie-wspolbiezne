using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkSum
{
    class Punkt8
    {
        static double calculatedValue(double value)
        {
            return (Math.Sin(Math.Sin(value * value) * Math.Cos(value * value) / Math.PI));
        }

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
            const int size = 5000000;
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"Zadanie 8: Benchmark z funkcja nieliniowa calculatedValue ({size} liczb, {cores} rdzeni)\n");

            int[] numbers = GenerateRandomArray(size);
            Stopwatch sw = new Stopwatch();

            // 1. Sekwencyjny foreach
            sw.Restart();
            double seqSum = 0;
            foreach (int number in numbers)
            {
                seqSum += calculatedValue(number);
            }
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjny foreach:         Suma = {seqSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms");

            // 2. Parallel.ForEach z lock
            object locker = new object();
            double parallelLockSum = 0;
            sw.Restart();
            Parallel.ForEach(numbers, number =>
            {
                double val = calculatedValue(number);
                lock (locker)
                {
                    parallelLockSum += val;
                }
            });
            sw.Stop();
            Console.WriteLine($"2. Parallel.ForEach (z lock):   Suma = {parallelLockSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (SPADEK)");

            // 3. PLINQ AsParallel().Sum()
            sw.Restart();
            double plinqSum = numbers.AsParallel().Sum(n => calculatedValue(n));
            sw.Stop();
            Console.WriteLine($"3. PLINQ AsParallel().Sum():    Suma = {plinqSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (PRZYSPIESZENIE)");

            // 4. Partycje Task.Run
            int chunkSize = size / cores;
            Task<double>[] tasks = new Task<double>[cores];
            sw.Restart();
            for (int i = 0; i < cores; i++)
            {
                int start = i * chunkSize;
                int end = (i == cores - 1) ? size : start + chunkSize;
                tasks[i] = Task.Run(() =>
                {
                    double localSum = 0;
                    for (int j = start; j < end; j++)
                    {
                        localSum += calculatedValue(numbers[j]);
                    }
                    return localSum;
                });
            }
            double[] partialResults = Task.WhenAll(tasks).Result;
            double tplSum = partialResults.Sum();
            sw.Stop();
            Console.WriteLine($"4. Partycje Task.Run:           Suma = {tplSum:F2} | Czas = {sw.ElapsedMilliseconds,5} ms (NAJSZYBSZE)");
        }
    }
}
