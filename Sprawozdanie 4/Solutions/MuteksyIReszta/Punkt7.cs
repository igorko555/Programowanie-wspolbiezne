using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MuteksyIReszta
{
    class Punkt7
    {
        static readonly int liczbaIteracji = 10000;
        static readonly int liczbaWatkow = 100;

        static int licznik = 0;
        static readonly object locker = new object();
        static readonly Mutex mutex = new Mutex();
        static readonly Semaphore semaphore = new Semaphore(1, 1);
        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        static void RunBenchmark(string nazwaWariantu, Action akcjaInkrementacji)
        {
            licznik = 0;
            List<Thread> watki = new List<Thread>();
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < liczbaWatkow; i++)
            {
                var watek = new Thread(() =>
                {
                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        akcjaInkrementacji();
                    }
                });
                watki.Add(watek);
                watek.Start();
            }

            foreach (var w in watki)
            {
                w.Join();
            }

            sw.Stop();
            Console.WriteLine($"Wariant: {nazwaWariantu,-22} | Licznik: {licznik,8} | Czas: {sw.ElapsedMilliseconds,5} ms");
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Rozpoczynam benchmark (watki: {liczbaWatkow}, iteracje: {liczbaIteracji}, oczekiwana suma: {liczbaWatkow * liczbaIteracji})\n");

            RunBenchmark("1. Brak synchronizacji", () =>
            {
                licznik++;
            });

            RunBenchmark("2. Lock (Monitor)", () =>
            {
                lock (locker)
                {
                    licznik++;
                }
            });

            RunBenchmark("3. Mutex", () =>
            {
                mutex.WaitOne();
                try
                {
                    licznik++;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            });

            RunBenchmark("4. Semaphore(1, 1)", () =>
            {
                semaphore.WaitOne();
                try
                {
                    licznik++;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            RunBenchmark("5. SemaphoreSlim(1, 1)", () =>
            {
                semaphoreSlim.Wait();
                try
                {
                    licznik++;
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            });

            Console.WriteLine("\nBenchmark zakonczony.");
        }
    }
}
