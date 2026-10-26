using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UnsafeCollection
{
    class Punkt4
    {
        static readonly object locker = new object();

        static void Main(string[] args)
        {
            List<Thread> watki = new List<Thread>();
            List<int> liczby = new List<int>(1000000);
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {
                    Random rand = new Random();
                    for (int l = 0; l < 10000; l++)
                    {
                        int val = rand.Next();
                        lock (locker)
                        {
                            liczby.Add(val);
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
            Console.WriteLine($"Czas wykonania (List + lock): {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Liczba elementow w liscie z lock: {liczby.Count} (oczekiwano: 1000000)");
        }
    }
}
