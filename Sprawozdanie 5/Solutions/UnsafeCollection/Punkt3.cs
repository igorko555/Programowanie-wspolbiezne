using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UnsafeCollection
{
    class Punkt3
    {
        static void Main(string[] args)
        {
            List<Thread> watki = new List<Thread>();
            ConcurrentBag<int> liczby = new ConcurrentBag<int>();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {
                    Random rand = new Random();
                    for (int l = 0; l < 10000; l++)
                    {
                        liczby.Add(rand.Next());
                    }
                });
                watki.Add(watek);
                watek.Start();
            }

            foreach (var watek in watki)
            {
                watek.Join();
            }

            sw.Stop();
            Console.WriteLine($"Czas wykonania (ConcurrentBag): {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Liczba elementow w ConcurrentBag: {liczby.Count} (oczekiwano: 1000000)");
        }
    }
}
