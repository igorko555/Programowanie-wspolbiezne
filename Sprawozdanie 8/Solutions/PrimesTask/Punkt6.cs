using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrimesTask
{
    public static class PrimesGenerator
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

        public static async Task<List<int>> FindPrimesAsync(int start, int end)
        {
            int cores = Environment.ProcessorCount;
            int rangeSize = (end - start) / cores;
            Task<List<int>>[] tasks = new Task<List<int>>[cores];

            for (int i = 0; i < cores; i++)
            {
                int s = start + i * rangeSize;
                int e = (i == cores - 1) ? end : s + rangeSize;
                tasks[i] = Task.Run(() => FindPrimesRange(s, e));
            }

            List<int>[] results = await Task.WhenAll(tasks);
            return results.SelectMany(primes => primes).ToList();
        }
    }

    class Punkt6
    {
        static void Main(string[] args)
        {
            const int start = 0;
            const int end = 10000000;
            Console.WriteLine($"Zadanie 6: PrimesGenerator (async/await z Task.WhenAll) w zakresie {start}..{end}\n");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> primes = PrimesGenerator.FindPrimesAsync(start, end).GetAwaiter().GetResult();
            sw.Stop();

            Console.WriteLine($"PrimesGenerator.FindPrimesAsync | Znaleziono: {primes.Count} liczb | Czas: {sw.ElapsedMilliseconds} ms");
        }
    }
}
