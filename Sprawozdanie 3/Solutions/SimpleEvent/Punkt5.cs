using System;
using System.Threading;

namespace SimpleEvent
{
    class Punkt5
    {
        static void Main(string[] args)
        {
            Alarm alarm = new Alarm(new TimeSpan(0, 0, 4), 500);
            alarm.ClockTicked += Alarm_ClockTicked;
            alarm.AlarmOccured += Alarm_AlarmOccured;

            Console.WriteLine("Uruchomiono Punkt 5 (Nowy Event ClockTicked oraz TimespanToAlarm).");
            Console.WriteLine("Rozpoczecie odliczania...");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
            else
            {
                Thread.Sleep(4500);
            }

            alarm.Dispose();
            Console.WriteLine("Koniec programu.");
        }

        private static void Alarm_ClockTicked(object? sender, ClockTickEventArgs e)
        {
            if (sender is Alarm a)
            {
                Console.WriteLine($"[Tick] Pozostalo: {e.RemainingTime.TotalSeconds:F1} s (TimespanToAlarm: {a.TimespanToAlarm.TotalSeconds:F1} s)");
            }
        }

        private static void Alarm_AlarmOccured(object sender, AlarmEventArgs args)
        {
            Console.WriteLine($"\n>>> {args.Message} at {args.AlarmFiredAt:HH:mm:ss.fff} <<<");
        }
    }
}
