using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UnsafeCollection
{
    class Punkt2
    {
        static void Main(string[] args)
        {
            List<Thread> watki = new List<Thread>();
            List<int> liczby = new List<int>(1000000);
            Random rand = new Random();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {
                    for (int l = 0; l < 10000; l++)
                    {
                        try
                        {
                            liczby.Add(rand.Next());
                        }
                        catch
                        {
                        }
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
            Console.WriteLine($"Czas wykonania: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Liczba elementow w liscie zwyklej: {liczby.Count} (oczekiwano: 1000000)");
        }
    }
}
