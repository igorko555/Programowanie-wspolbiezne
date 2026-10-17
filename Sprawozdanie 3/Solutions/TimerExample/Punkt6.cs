using System;
using System.Threading;
using S = System.Timers;
using T = System.Threading;

namespace TimerExample
{
    class TimerState
    {
        public int Counter;
    }

    class Punkt6
    {
        static void Main(string[] args)
        {
            TimerState timerState = new TimerState { Counter = 0 };
            T.Timer timerThreading = new T.Timer(new T.TimerCallback(TimerTask), timerState, 2000, 1000);

            S.Timer timerSystem = new S.Timer(1500);
            timerSystem.AutoReset = false;
            timerSystem.Elapsed += TimerSystem_Elapsed;
            timerSystem.Start();

            Console.WriteLine("Uruchomiono Punkt 6 (AutoReset = false oraz TimerTask z Interlocked).");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
            else
            {
                Thread.Sleep(4000);
            }

            timerThreading.Dispose();
            timerSystem.Dispose();
            Console.WriteLine("Koniec programu.");
        }

        private static void TimerSystem_Elapsed(object? sender, S.ElapsedEventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] System timer tick tock (jednorazowy budzik)...");
        }

        private static void TimerTask(object? timerState)
        {
            var state = timerState as TimerState;
            if (state == null) return;
            int c = T.Interlocked.Increment(ref state.Counter);
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: starting a new callback. Counter = {c}");
        }
    }
}
