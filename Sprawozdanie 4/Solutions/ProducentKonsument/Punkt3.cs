using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducentKonsument
{
    class Punkt3
    {
        static int licznik = 0;
        static readonly int liczbaWatkow = 5;
        static readonly int liczbaIteracji = 5;

        static EventWaitHandle pisanie = new AutoResetEvent(true);
        static EventWaitHandle czytanie = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            List<Thread> watkiKonsument = new List<Thread>();

            for (int i = 0; i < liczbaWatkow; i++)
            {
                var konsument = new Thread(() =>
                {
                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        czytanie.WaitOne();
                        Console.WriteLine($"Konsument [{Thread.CurrentThread.ManagedThreadId}] pobral wartosc: {licznik}");
                        pisanie.Set();
                    }
                });
                watkiKonsument.Add(konsument);
                konsument.Start();
            }

            Thread watekProducent = new Thread(() =>
            {
                int calkowitaLiczbaZadan = liczbaWatkow * liczbaIteracji;
                for (int k = 1; k <= calkowitaLiczbaZadan; k++)
                {
                    pisanie.WaitOne();
                    licznik = k;
                    Console.WriteLine($"Producent wygenerowal wartosc: {licznik}");
                    czytanie.Set();
                }
            });

            watekProducent.Start();

            foreach (var watek in watkiKonsument)
            {
                watek.Join();
            }

            watekProducent.Join();

            Console.WriteLine($"Koniec programu. Wszystkie dane zostaly przeslane bez strat.");
        }
    }
}
