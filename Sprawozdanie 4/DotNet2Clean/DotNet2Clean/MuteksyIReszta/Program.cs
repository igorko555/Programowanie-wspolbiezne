using System;
using System.Collections.Generic;
using System.Threading;

namespace MuteksyIReszta
{
    class Program
    {
        static List<Thread> watki = new List<Thread>();

        static readonly int liczbaIteracji = 10000;
        static readonly int liczbaWatkow = 100;

        static int licznik = 0;

        static void Main(string[] args)
        {
            #region Bez synchronizacji 

            for (int i = 0; i < liczbaWatkow; i++)
            {
                var watek = new Thread((o) =>
                {
                    int nrWatku = (int)o;

                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        licznik++;
                    }

                });
                watki.Add(watek);
                watek.Start(i);
            }

            foreach (var w in watki)
            {
                w.Join();
            }

            Console.WriteLine($"Licznik: {licznik}");
            #endregion

            Console.WriteLine("To już jest koniec, naciśnij ENTER...");
            Console.ReadLine();
        }
    }
}
