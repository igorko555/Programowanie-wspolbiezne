using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerQ
{
    class Punkt3
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kolejka wspolbiezna ConcurrentQueue sterowana SemaphoreSlim\n");

            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            SemaphoreSlim availableItems = new SemaphoreSlim(0);
            List<Thread> consumers = new List<Thread>();
            Random rand = new Random();

            var producer = new Thread(() =>
            {
                for (int l = 0; l < 50; l++)
                {
                    queue.Enqueue(l);
                    Console.WriteLine($"[Producent] Wstawiono: {l}");
                    availableItems.Release();
                    Thread.Sleep(rand.Next(10, 50));
                }
            });
            producer.Start();

            for (int i = 0; i < 5; i++)
            {
                var consumer = new Thread(() =>
                {
                    for (int l = 0; l < 10; l++)
                    {
                        availableItems.Wait();
                        if (queue.TryDequeue(out int item))
                        {
                            Console.WriteLine($"[Konsument {Thread.CurrentThread.ManagedThreadId}] Pobrano element: {item}");
                        }
                        Thread.Sleep(rand.Next(30, 80));
                    }
                });
                consumers.Add(consumer);
                consumer.Start();
            }

            foreach (var c in consumers) c.Join();
            producer.Join();
            Console.WriteLine("\nTest Punkt 3 zakonczony pomyslnie.");
        }
    }
}
