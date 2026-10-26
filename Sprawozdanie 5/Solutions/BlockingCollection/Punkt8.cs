using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BlockingCollectionDemo
{
    class Punkt8
    {
        static void Main(string[] args)
        {
            BlockingCollection<int> buffer = new BlockingCollection<int>(10);
            List<Thread> watki = new List<Thread>();

            var producent = new Thread(() =>
            {
                for (int l = 0; l < 100; l++)
                {
                    buffer.Add(l);
                    Console.WriteLine($"Producent wstawia: {l}");
                    Thread.Sleep(50);
                }
                buffer.CompleteAdding();
            });
            producent.Start();

            Thread.Sleep(5000);

            for (int i = 0; i < 10; i++)
            {
                int id = i;
                var watek = new Thread(() =>
                {
                    Random rand = new Random();
                    for (int l = 0; l < 10; l++)
                    {
                        try
                        {
                            int item = buffer.Take();
                            Console.WriteLine($"Konsument [{id}] pobral: {item}");
                        }
                        catch (InvalidOperationException)
                        {
                            break;
                        }
                        Thread.Sleep(rand.Next(500));
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
            Console.WriteLine("Praca bufora zakonczona.");
        }
    }
}
