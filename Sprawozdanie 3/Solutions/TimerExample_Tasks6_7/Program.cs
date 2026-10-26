using System;
using S = System.Timers;
using T = System.Threading;

namespace TimerExample_Tasks6_7
{
    /// <summary>
    /// Klasa przechowująca stan przekazywany do wywołania zwrotnego timera (Zadanie 6).
    /// </summary>
    public class TimerState
    {
        // Licznik wywołań chroniony operacjami atomowymi Interlocked
        public int Counter;

        // Referencja do instancji timera, umożliwiająca jego wyłączenie od wewnątrz (Zadanie 7)
        public T.Timer? AssociatedTimer;
    }

    class Program
    {
        private static T.Timer? timerThreading;
        private static S.Timer? timerSystem;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================================");
            Console.WriteLine("   LABORATORIUM 3 - ZADANIA 6-7: CZASOMIERZE I ATOMOWE LICZNIKI   ");
            Console.WriteLine("==================================================================\n");

            // ------------------------------------------------------------------------------------------
            // ZADANIE 6: System.Timers.Timer jako jednorazowy budzik (AutoReset = false)
            // ------------------------------------------------------------------------------------------
            Console.WriteLine("--- 1. INICJALIZACJA System.Timers.Timer (Interwał: 1,5s) ---");
            timerSystem = new S.Timer(1500);
            timerSystem.Elapsed += TimerSystem_Elapsed;

            // Linijka wymagana w instrukcji (Zadanie 6):
            timerSystem.AutoReset = false; // Powoduje, że timer wyzwala zdarzenie dokładnie jeden raz
            timerSystem.Start();
            Console.WriteLine("[System.Timers.Timer] Uruchomiony z AutoReset = false (zadziała jednokrotnie jako budzik).\n");

            // ------------------------------------------------------------------------------------------
            // ZADANIE 6: System.Threading.Timer z obiektem stanu TimerState i metodą TimerTask
            // ------------------------------------------------------------------------------------------
            Console.WriteLine("--- 2. INICJALIZACJA System.Threading.Timer (Start po 2s, takt co 1s) ---");
            TimerState timerState = new TimerState { Counter = 0 };

            // Uruchomienie z opóźnieniem początkowym 2000 ms i okresem 1000 ms (dla szybszego testu laboratoryjnego)
            timerThreading = new T.Timer(new T.TimerCallback(TimerTask), timerState, 2000, 1000);
            timerState.AssociatedTimer = timerThreading;

            Console.WriteLine("[System.Threading.Timer] Oczekiwanie na cykliczne wywołania TimerTask...");
            Console.WriteLine("Timer zostanie automatycznie wyłączony po 5 wykonaniach (Zadanie 7).\n");

            // Oczekiwanie w wątku głównym na zakończenie demonstracji
            // 2s opóźnienia + 5 wywołań po 1s = ok. 7-8 sekund
            T.Thread.Sleep(9000);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n[STAN KOŃCOWY]: timerThreading Counter = {timerState.Counter}");
            Console.ResetColor();

            Console.WriteLine("Zakończenie demonstracji (Naciśnij klawisz lub zaczekaj)...");
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }

            // Zwolnienie zasobów czasomierzy
            timerThreading?.Dispose();
            timerSystem?.Dispose();
        }

        /// <summary>
        /// Metoda obsługi zdarzenia System.Timers.Timer (jednorazowe wywołanie).
        /// </summary>
        private static void TimerSystem_Elapsed(object? sender, S.ElapsedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [System.Timers.Timer] BUDZIK ZADZWONIŁ (Elapsed). Ponowne wywołanie zablokowane przez AutoReset=false.");
            Console.ResetColor();
        }

        /// <summary>
        /// Metoda wywoływana cyklicznie przez System.Threading.Timer w osobnym wątku z puli wątków (Zadanie 6 & 7).
        /// </summary>
        private static void TimerTask(object? timerState)
        {
            var state = timerState as TimerState;
            if (state == null) return;

            // ------------------------------------------------------------------------------------------
            // ZADANIE 6: Atomowa inkrementacja licznika w środowisku wielowątkowym
            // ------------------------------------------------------------------------------------------
            int currentVal = T.Interlocked.Increment(ref state.Counter);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff} | Wątek {T.Thread.CurrentThread.ManagedThreadId}]: TimerTask callback executed. Licznik Counter = {currentVal}");

            // ------------------------------------------------------------------------------------------
            // ZADANIE 7: Wyłączenie timera po 5 wywołaniach (gdy licznik osiągnie 5)
            // Używamy timerThreading.Change(T.Timeout.Infinite, 0);
            // ------------------------------------------------------------------------------------------
            if (currentVal >= 5)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] >>> [ZADANIE 7]: Osiągnięto {currentVal} wywołań! Wyłączanie timera przez Change(Timeout.Infinite, 0) <<<");
                Console.ResetColor();

                state.AssociatedTimer?.Change(T.Timeout.Infinite, 0);
            }
        }
    }
}
