using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrimesTask
{
    public static class PrimesGenerator
    {
        public static bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            int limit = (int)Math.Sqrt(number);
            for (int i = 3; i <= limit; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static List<int> FindPrimesRange(int start, int end)
        {
            List<int> primes = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (IsPrime(i)) primes.Add(i);
            }
            return primes;
        }

        public static async Task<List<int>> FindPrimesAsync(int start, int end, int partitions)
        {
            int rangeSize = (end - start) / partitions;
            Task<List<int>>[] tasks = new Task<List<int>>[partitions];

            for (int p = 0; p < partitions; p++)
            {
                int pStart = start + p * rangeSize;
                int pEnd = (p == partitions - 1) ? end : pStart + rangeSize;
                tasks[p] = Task.Run(() => FindPrimesRange(pStart, pEnd));
            }

            List<int>[] results = await Task.WhenAll(tasks);
            return results.SelectMany(r => r).ToList();
        }
    }

    class Punkt3
    {
        static void Main(string[] args)
        {
            const int limit = 500000;
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"Wyszukiwanie liczb pierwszych w zakresie 2..{limit} (Rdzenie CPU: {cores})\n");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> seqPrimes = PrimesGenerator.FindPrimesRange(2, limit);
            sw.Stop();
            Console.WriteLine($"1. Sekwencyjnie        | Znaleziono: {seqPrimes.Count,6} | Czas: {sw.ElapsedMilliseconds,5} ms");

            sw.Restart();
            int partSize = limit / cores;
            Task<List<int>>[] tasks = new Task<List<int>>[cores];
            for (int i = 0; i < cores; i++)
            {
                int start = 2 + i * partSize;
                int end = (i == cores - 1) ? limit : start + partSize;
                tasks[i] = Task.Factory.StartNew(() => PrimesGenerator.FindPrimesRange(start, end));
            }

            Task<List<int>> continuation = Task.Factory.ContinueWhenAll(tasks, completedTasks =>
            {
                List<int> combined = new List<int>();
                foreach (var t in completedTasks)
                {
                    combined.AddRange(t.Result);
                }
                return combined;
            });

            List<int> tplPrimes = continuation.Result;
            sw.Stop();
            Console.WriteLine($"2. TPL ContinueWhenAll | Znaleziono: {tplPrimes.Count,6} | Czas: {sw.ElapsedMilliseconds,5} ms");

            sw.Restart();
            List<int> asyncPrimes = PrimesGenerator.FindPrimesAsync(2, limit, cores).GetAwaiter().GetResult();
            sw.Stop();
            Console.WriteLine($"3. Async / WhenAll     | Znaleziono: {asyncPrimes.Count,6} | Czas: {sw.ElapsedMilliseconds,5} ms");
        }
    }
}
