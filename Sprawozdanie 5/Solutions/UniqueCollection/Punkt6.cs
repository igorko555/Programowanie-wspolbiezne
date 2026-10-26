using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UniqueCollection
{
    class Punkt6
    {
        static void Main(string[] args)
        {
            List<Thread> watki = new List<Thread>();
            ConcurrentDictionary<int, byte> dict = new ConcurrentDictionary<int, byte>();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 100; i++)
            {
                var watek = new Thread(() =>
                {
                    Random rand = new Random();
                    for (int l = 0; l < 100; l++)
                    {
                        int liczba = rand.Next(100000);
                        while (!dict.TryAdd(liczba, 0))
                        {
                            liczba = rand.Next(100000);
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
            Console.WriteLine($"Liczba unikalnych elementow w ConcurrentDictionary: {dict.Count} (oczekiwano: 10000)");
            Console.WriteLine("Wszystkie elementy w slowniku sa unikalne z definicji klucza.");
        }
    }
}
