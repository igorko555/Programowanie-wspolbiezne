using System;
using System.Threading;
using T = System.Threading;

namespace TimerExample
{
    class TimerState
    {
        public int Counter;
        public T.Timer? TimerRef;
    }

    class Punkt7
    {
        static void Main(string[] args)
        {
            TimerState timerState = new TimerState { Counter = 0 };
            T.Timer timerThreading = new T.Timer(new T.TimerCallback(TimerTask), timerState, 1000, 1000);
            timerState.TimerRef = timerThreading;

            Console.WriteLine("Uruchomiono Punkt 7 (Wyłączenie timera po 5 wywołaniach).");
            Console.WriteLine("Oczekiwanie na automatyczne wyłączenie timera...");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
            else
            {
                Thread.Sleep(6500);
            }

            timerThreading.Dispose();
            Console.WriteLine("Koniec programu.");
        }

        private static void TimerTask(object? timerState)
        {
            var state = timerState as TimerState;
            if (state == null) return;

            int c = T.Interlocked.Increment(ref state.Counter);
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: callback nr {c}");

            if (c >= 5)
            {
                Console.WriteLine("Osiągnięto 5 wywołań. Wyłączam timer za pomocą timerThreading.Change(Timeout.Infinite, 0)...");
                state.TimerRef?.Change(T.Timeout.Infinite, 0);
            }
        }
    }
}
