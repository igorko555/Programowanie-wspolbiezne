using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducentKonsument
{
    class Punkt2
    {
        static int licznik = 0;
        static readonly int liczbaWatkow = 10;
        static readonly int liczbaIteracji = 10;

        static void Main(string[] args)
        {
            List<Thread> watkiKonsument = new List<Thread>();

            for (int i = 0; i < liczbaWatkow; i++)
            {
                var konsument = new Thread(() =>
                {
                    Random rand = new Random(Thread.CurrentThread.ManagedThreadId);

                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        var czasUspienia = rand.Next(100, 300);
                        Thread.Sleep(czasUspienia);
                        Console.WriteLine($"Konsument [{Thread.CurrentThread.ManagedThreadId}] odczytal licznik: {licznik} (sleep: {czasUspienia}ms)");
                    }
                });
                watkiKonsument.Add(konsument);
                konsument.Start();
            }

            Thread watekProducent = new Thread(() =>
            {
                Random randPiszacy = new Random(Thread.CurrentThread.ManagedThreadId);
                try
                {
                    while (true)
                    {
                        Thread.Sleep(randPiszacy.Next(50, 150));
                        Interlocked.Increment(ref licznik);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("\nWatek piszacy zostal zakonczony.");
                }
            });

            watekProducent.Start();

            foreach (var watek in watkiKonsument)
            {
                watek.Join();
            }

            watekProducent.Interrupt();
            watekProducent.Join();

            Console.WriteLine($"Koniec programu. Ostateczna wartosc licznika: {licznik}");
        }
    }
}
