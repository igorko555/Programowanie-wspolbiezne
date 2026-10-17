using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BlockingCollectionDemo
{
    class Punkt4
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ograniczony bufor blokujacy BlockingCollection (pojemnosc: 10)\n");

            BlockingCollection<int> buffer = new BlockingCollection<int>(boundedCapacity: 10);
            List<Thread> consumers = new List<Thread>();

            var producer = new Thread(() =>
            {
                for (int i = 1; i <= 30; i++)
                {
                    Console.WriteLine($"[Producent] Proba dodania {i} (w buforze: {buffer.Count}/10)...");
                    buffer.Add(i);
                    Console.WriteLine($"[Producent] Sukces: dodano {i}");
                    Thread.Sleep(50);
                }
                buffer.CompleteAdding();
                Console.WriteLine("[Producent] Zakonczono dodawanie (CompleteAdding).");
            });

            producer.Start();

            Thread.Sleep(1000);

            for (int c = 0; c < 3; c++)
            {
                var consumer = new Thread(() =>
                {
                    try
                    {
                        foreach (var item in buffer.GetConsumingEnumerable())
                        {
                            Console.WriteLine($"[Konsument {Thread.CurrentThread.ManagedThreadId}] Pobrano {item} (pozostalo: {buffer.Count})");
                            Thread.Sleep(150);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                    }
                });
                consumers.Add(consumer);
                consumer.Start();
            }

            producer.Join();
            foreach (var c in consumers) c.Join();
            Console.WriteLine("\nTest Punkt 4 zakonczony pomyslnie.");
        }
    }
}
