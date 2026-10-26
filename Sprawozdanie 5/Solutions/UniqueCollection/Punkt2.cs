using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace UniqueCollection
{
    class Punkt2
    {
        static readonly int threadCount = 100;
        static readonly int itemsPerThread = 100;
        static readonly object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine($"Weryfikacja unikalnosci 10000 liczb (100 watkow x 100 elementow z zakresu 0..10000)\n");

            ConcurrentBag<int> bagUnsafe = new ConcurrentBag<int>();
            List<Thread> threads = new List<Thread>();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        int number = rand.Next(10001);
                        while (bagUnsafe.Any(x => x == number))
                        {
                            number = rand.Next(10001);
                        }
                        bagUnsafe.Add(number);
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var t in threads) t.Join();
            sw.Stop();
            Console.WriteLine($"1. ConcurrentBag (Any + Add)     | Unikalne: {bagUnsafe.IsUnique(),-5} | Czas: {sw.ElapsedMilliseconds,5} ms | Elementow: {bagUnsafe.Count}");

            ConcurrentBag<int> bagLocked = new ConcurrentBag<int>();
            threads.Clear();
            sw.Restart();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        lock (locker)
                        {
                            int number = rand.Next(10001);
                            while (bagLocked.Any(x => x == number))
                            {
                                number = rand.Next(10001);
                            }
                            bagLocked.Add(number);
                        }
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var t in threads) t.Join();
            sw.Stop();
            Console.WriteLine($"2. ConcurrentBag z ryglem lock   | Unikalne: {bagLocked.IsUnique(),-5} | Czas: {sw.ElapsedMilliseconds,5} ms | Elementow: {bagLocked.Count}");

            ConcurrentDictionary<int, byte> dict = new ConcurrentDictionary<int, byte>();
            threads.Clear();
            sw.Restart();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        int number = rand.Next(10001);
                        while (!dict.TryAdd(number, 0))
                        {
                            number = rand.Next(10001);
                        }
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var t in threads) t.Join();
            sw.Stop();
            bool dictUnique = dict.Keys.Distinct().Count() == dict.Count;
            Console.WriteLine($"3. ConcurrentDictionary (TryAdd) | Unikalne: {dictUnique,-5} | Czas: {sw.ElapsedMilliseconds,5} ms | Elementow: {dict.Count}");
            Console.WriteLine("\nTest Punkt 2 zakonczony.");
        }
    }
}
