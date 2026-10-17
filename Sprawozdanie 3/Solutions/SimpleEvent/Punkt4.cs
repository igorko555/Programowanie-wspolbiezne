using System;
using System.Threading;

namespace SimpleEvent
{
    class Punkt4
    {
        static void Main(string[] args)
        {
            int customInterval = 250;
            Alarm alarm = new Alarm(new TimeSpan(0, 0, 3), customInterval);
            alarm.AlarmOccured += Alarm_AlarmOccured;

            Console.WriteLine("Uruchomiono Punkt 4 (Parametryzacja checkingInterval).");
            Console.WriteLine($"Ustawiony checkingInterval: {alarm.CheckingInterval} ms.");
            Console.WriteLine("Oczekiwanie na alarm...");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
            else
            {
                Thread.Sleep(3500);
            }

            alarm.Dispose();
            Console.WriteLine("Koniec programu.");
        }

        private static void Alarm_AlarmOccured(object sender, AlarmEventArgs args)
        {
            Console.WriteLine($"{args.Message} at {args.AlarmFiredAt}");
        }
    }
}
