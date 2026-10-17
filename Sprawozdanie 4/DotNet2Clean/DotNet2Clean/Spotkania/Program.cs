using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Spotkania
{
    class Program
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
                    Thread.Sleep(rand.Next(2000));
                    // WaitHandle.SignalAndWait(wh1, wh2);
                    Console.WriteLine("tadam wątek 1 iteracja " + i);
                }
            });
            var watek2 = new Thread(() =>
            {
                for (int i = 0; i < liczbaIteracji; i++)
                {
                    Thread.Sleep(rand.Next(2000));
                    //  WaitHandle.SignalAndWait(wh2, wh1);
                    Console.WriteLine("tadam wątek 2 iteracja " + i);
                }
            });
            watek1.Start();
            watek2.Start();
            watek1.Join();
            watek2.Join();
            Console.WriteLine("To jest już koniec. Naciśnij ENTER...");
            Console.ReadLine();
        }
    }
}