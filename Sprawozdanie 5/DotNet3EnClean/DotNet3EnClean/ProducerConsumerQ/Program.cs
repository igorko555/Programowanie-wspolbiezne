using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerQ
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentQueue<int> cq = new ConcurrentQueue<int>();
            SemaphoreSlim sem = new SemaphoreSlim(0);
            List<Thread> watki = new List<Thread>();
            Random rand = new Random();

            var producent = new Thread(() =>
            {
                for (int l = 0; l < 100; l++)
                {
                    cq.Enqueue(l);
                    Console.WriteLine($"Wkladam {l}");
                    sem.Release();
                    Thread.Sleep(rand.Next(10));
                }
            });
            producent.Start();

            for (int i = 0; i < 10; i++)
            {
                var watek = new Thread(() =>
                {
                    for (int l = 0; l < 10; l++)
                    {
                        sem.Wait();
                        if (cq.TryDequeue(out int result2))
                        {
                            Console.WriteLine($"Odbieram {result2}");
                        }
                        Thread.Sleep(rand.Next(20));
                    }
                });
                watki.Add(watek);
                watek.Start();
            }

            foreach (var watek in watki)
            {
                watek.Join();
            }
            producent.Join();
            Console.WriteLine("Wszystkie elementy zostaly poprawnie odebrane.");
        }
    }
}
