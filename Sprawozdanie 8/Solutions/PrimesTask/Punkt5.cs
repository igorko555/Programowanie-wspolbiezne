using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrimesTask
{
    class Punkt5
    {
        public static bool IsPrime(int liczba)
        {
            if (liczba <= 1) return false;
            if (liczba <= 3) return true;
            if (liczba % 2 == 0 || liczba % 3 == 0) return false;
            for (int i = 5; i * i <= liczba; i += 6)
            {
                if (liczba % i == 0 || liczba % (i + 2) == 0) return false;
            }
            return true;
        }

        static List<int> FindPrimesRange(int start, int end)
        {
            List<int> primes = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (IsPrime(i)) primes.Add(i);
            }
            return primes;
        }

        static void Main(string[] args)
        {
            const int countFromTotal = 0;
            const int countToTotal = 10000000;
            int cores = Environment.ProcessorCount;

            Console.WriteLine($"Liczby pierwsze w zakresie {countFromTotal}..{countToTotal} (Rdzenie: {cores})\n");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> singleThreadPrimes = FindPrimesRange(countFromTotal, countToTotal);
            sw.Stop();
            Console.WriteLine($"1. Pojedynczy watek  | Znaleziono: {singleThreadPrimes.Count} | Czas: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            int rangeSize = (countToTotal - countFromTotal) / cores;
            Task<List<int>>[] tasks = new Task<List<int>>[cores];
            for (int i = 0; i < cores; i++)
            {
                int s = countFromTotal + i * rangeSize;
                int e = (i == cores - 1) ? countToTotal : s + rangeSize;
                tasks[i] = Task.Factory.StartNew(() => FindPrimesRange(s, e));
            }

            Task<List<int>> finalTask = Task.Factory.ContinueWhenAll(tasks, completedTasks =>
            {
                List<int> combined = new List<int>();
                foreach (var t in completedTasks)
                {
                    combined.AddRange(t.Result);
                }
                return combined;
            });

            List<int> multiThreadPrimes = finalTask.Result;
            sw.Stop();
            Console.WriteLine($"2. ContinueWhenAll   | Znaleziono: {multiThreadPrimes.Count} | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
