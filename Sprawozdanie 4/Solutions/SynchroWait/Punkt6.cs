using System;
using System.Collections.Generic;
using System.Threading;

namespace SynchroWait
{
    class Punkt6
    {
        static readonly int liczbaIteracji = 5;
        static readonly int liczbaWatkow = 10;
        static WaitHandle[] handles = new WaitHandle[liczbaWatkow];
        static List<Thread> watki = new List<Thread>();
        static readonly Random rand = Random.Shared;

        static void Main(string[] args)
        {
            for (int i = 0; i < liczbaWatkow; i++)
            {
                handles[i] = new ManualResetEvent(false);
            }

            for (int i = 0; i < liczbaWatkow; i++)
            {
                var watek = new Thread((o) =>
                {
                    int nrWatku = (int)o;
                    Thread.Sleep(rand.Next(50, 200));
                    Console.WriteLine($"Watek nr {nrWatku} wystartowal.");

                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        Console.WriteLine($"Watek nr {nrWatku} wykonuje krok {j}");
                        Thread.Sleep(rand.Next(50, 150));
                    }

                    Console.WriteLine($"Watek nr {nrWatku} zakonczyl prace i sygnalizuje uchwyt.");
                    ((ManualResetEvent)handles[nrWatku]).Set();
                });

                watki.Add(watek);
                watek.Start(i);
            }

            int firstFinished = WaitHandle.WaitAny(handles);
            Console.WriteLine($"---------------- Doczekalismy sie pierwszego watku (nr {firstFinished}) -----------------");

            WaitHandle.WaitAll(handles);
            Console.WriteLine("---------------- Doczekalismy sie wszystkich watkow -----------------");

            foreach (var w in watki)
            {
                w.Join();
            }

            Console.WriteLine("Koniec programu SynchroWait.");
        }
    }
}
