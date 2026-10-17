using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UnsafeCollection
{
    class Punkt1
    {
        static readonly int threadCount = 100;
        static readonly int itemsPerThread = 10000;
        static readonly object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine($"Wielowatkowy test kolekcji (watki: {threadCount}, elementy/watek: {itemsPerThread})\n");

            List<Thread> threads = new List<Thread>();
            List<int> unsafeList = new List<int>();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        try
                        {
                            unsafeList.Add(rand.Next());
                        }
                        catch
                        {
                        }
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var w in threads) w.Join();
            sw.Stop();
            Console.WriteLine($"1. List<int> bez synchronizacji  | Elementow: {unsafeList.Count,8} (oczekiwano 1000000) | Czas: {sw.ElapsedMilliseconds,5} ms");

            threads.Clear();
            List<int> lockedList = new List<int>();
            sw.Restart();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        int val = rand.Next();
                        lock (locker)
                        {
                            lockedList.Add(val);
                        }
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var w in threads) w.Join();
            sw.Stop();
            Console.WriteLine($"2. List<int> z blokada lock      | Elementow: {lockedList.Count,8} (oczekiwano 1000000) | Czas: {sw.ElapsedMilliseconds,5} ms");

            threads.Clear();
            ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();
            sw.Restart();

            for (int i = 0; i < threadCount; i++)
            {
                var w = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
                    for (int j = 0; j < itemsPerThread; j++)
                    {
                        concurrentBag.Add(rand.Next());
                    }
                });
                threads.Add(w);
                w.Start();
            }

            foreach (var w in threads) w.Join();
            sw.Stop();
            Console.WriteLine($"3. ConcurrentBag<int> (lock-free)| Elementow: {concurrentBag.Count,8} (oczekiwano 1000000) | Czas: {sw.ElapsedMilliseconds,5} ms");
            Console.WriteLine("\nTest Punkt 1 zakonczony.");
        }
    }
}
