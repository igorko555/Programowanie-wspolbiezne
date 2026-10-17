using System;
using System.Collections.Generic;
using System.Threading;

namespace SynchroWait
{
    class Program
    {

        static List<Thread> watki = new List<Thread>();
        static readonly int liczbaIteracji = 5;
        static readonly int liczbaWatkow = 10;
        static Random rand = new Random();

        static void Main(string[] args)
        {
            for (int i = 0; i < liczbaWatkow; i++)
            {
                var watek = new Thread((o) =>
                {
                    int nrWatku = (int)o;
                    Thread.Sleep(rand.Next(1000));
                    Console.WriteLine($"Wątek nr {nrWatku} wystartował");
                    for (int j = 0; j < liczbaIteracji; j++)
                    {

                        Console.WriteLine($"Wątek nr {nrWatku} pracuje w iteracji nr {j}");
                        Thread.Sleep(rand.Next(1000));
                    }
                    Thread.Sleep(rand.Next(1000));
                    Console.WriteLine($"Wątek nr {nrWatku} zakończył działanie");
                });
                watki.Add(watek);
                watek.Start(i);
            }

            Console.WriteLine("---------------- Doczekaliśmy się -----------------");

            foreach (var w in watki)
            {
                w.Join();
            }

            Console.WriteLine("To już jest koniec, naciśnij ENTER...");
            Console.ReadLine();
        }
    }
}