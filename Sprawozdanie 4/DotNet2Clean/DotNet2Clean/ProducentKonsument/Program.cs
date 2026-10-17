using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducentKonsument
{
    class Program
    {

        static int licznik = 0;
        static readonly int liczbaWatkow = 10;
        static readonly int liczbaIteracji = 10;


        static void Main(string[] args)
        {
            List<Thread> watkiKonsument = new List<Thread>();
            //Random rand = new Random(); //random wspólny
            for (int i = 0; i < liczbaWatkow; i++)
            {
                var konsument = new Thread(() =>
                {
                    Random rand = new Random(); //random lokalny seed oparty na czasie
                    //Random rand = new Random(Thread.CurrentThread.ManagedThreadId); //random lokalny seed oparty na Id wątku

                    for (int j = 0; j < liczbaIteracji; j++)
                    {
                        var czasUspienia = rand.Next(1000);
                        Console.Write($"{czasUspienia} ");
                        Thread.Sleep(czasUspienia);
                        Console.WriteLine($"[{licznik}] ");
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
                        Thread.Sleep(randPiszacy.Next(100));
                        Interlocked.Increment(ref licznik);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("\nWątek piszący został zakończony");
                }
            });

            watekProducent.Start();

            foreach (var watek in watkiKonsument)
            {
                watek.Join();
            }

            watekProducent.Interrupt();
            watekProducent.Join();

            Console.WriteLine("To już jest koniec, naciśnij ENTER...");
            Console.ReadLine();
        }
    }
}