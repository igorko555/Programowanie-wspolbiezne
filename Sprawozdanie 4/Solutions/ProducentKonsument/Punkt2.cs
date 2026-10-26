using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducentKonsument
{
    class Punkt2
    {
        const int tryb = 2;
        static int licznik = 0;
        static readonly Random wspolnyRandom = new Random();

        static void Main(string[] args)
        {
            List<Thread> konsumenci = new List<Thread>();

            for (int i = 0; i < 10; i++)
            {
                Thread konsument = new Thread(() =>
                {
                    Random rand = tryb switch
                    {
                        1 => new Random(),
                        2 => new Random(Thread.CurrentThread.ManagedThreadId),
                        _ => wspolnyRandom
                    };

                    for (int j = 0; j < 10; j++)
                    {
                        int opoznienie = rand.Next(100, 300);
                        Thread.Sleep(opoznienie);

                        Console.WriteLine(
                            $"Watek {Thread.CurrentThread.ManagedThreadId}: " +
                            $"licznik = {licznik}, sleep = {opoznienie} ms");
                    }
                });

                konsumenci.Add(konsument);
                konsument.Start();
            }

            Thread producent = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        Interlocked.Increment(ref licznik);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Producent zakonczyl prace.");
                }
            });

            producent.Start();

            foreach (Thread konsument in konsumenci)
                konsument.Join();

            producent.Interrupt();
            producent.Join();

            Console.WriteLine("Koniec programu.");
        }
    }
}