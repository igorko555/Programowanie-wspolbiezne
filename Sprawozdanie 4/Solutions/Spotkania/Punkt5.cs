using System;
using System.Threading;

namespace Spotkania
{
    class Punkt5
    {
        private static readonly EventWaitHandle wh1 = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static readonly EventWaitHandle wh2 = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static readonly int liczbaIteracji = 5;
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            var watek1 = new Thread(() =>
            {
                for (int i = 0; i < liczbaIteracji; i++)
                {
                    int delay = rand.Next(200, 600);
                    Thread.Sleep(delay);
                    WaitHandle.SignalAndWait(wh1, wh2);
                    Console.WriteLine($"Tadam watek 1 dotarl do punktu spotkania w iteracji {i} (sleep: {delay}ms)");
                }
            });

            var watek2 = new Thread(() =>
            {
                for (int i = 0; i < liczbaIteracji; i++)
                {
                    int delay = rand.Next(700, 1500);
                    Thread.Sleep(delay);
                    WaitHandle.SignalAndWait(wh2, wh1);
                    Console.WriteLine($"Tadam watek 2 dotarl do punktu spotkania w iteracji {i} (sleep: {delay}ms)");
                }
            });

            watek1.Start();
            watek2.Start();

            watek1.Join();
            watek2.Join();

            Console.WriteLine("Koniec demonstracji wzorca Rendezvous.");
        }
    }
}
