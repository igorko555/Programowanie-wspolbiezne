using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace UniqueCollection
{
    class Punkt5
    {
        static readonly object locker = new object();

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
                    for (int l = 0; l < 100; l++)
                    {
                        int liczba = rand.Next(10001);
                        lock (locker)
                        {
                            while (liczby.Any(x => x == liczba))
                            {
                                liczba = rand.Next(10001);
                            }
                            liczby.Add(liczba);
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
            Console.WriteLine($"Uplynelo {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Liczba elementow: {liczby.Count}");
            if (liczby.IsUnique())
            {
                Console.WriteLine("wszystkie liczby sa unikalne");
            }
            else
            {
                Console.WriteLine("Nie wszystkie liczby sa unikalne");
            }
        }
    }
}
