using System;
using S = System.Timers;
using T = System.Threading;

namespace TimerExample
{
    class Program
    {
      
        static void Main(string[] args)
        {
            T.Timer timerThreading = new T.Timer(t=>Console.WriteLine("Timer Threading tick tock"), null, 5000, 1000);
            S.Timer timerSystem = new S.Timer(1500);
            timerSystem.Elapsed += TimerSystem_Elapsed;
            timerSystem.Start();
              
            Console.ReadLine();
        }

        private static void TimerSystem_Elapsed(object sender, S.ElapsedEventArgs e)
        {
            Console.WriteLine("System timer tick tock...");
        }
              
    }
}
