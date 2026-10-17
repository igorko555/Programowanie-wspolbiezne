using System;
using System.Threading;

namespace SimpleEvent
{
    class Punkt3
    {
        static void Main(string[] args)
        {
            Alarm alarm = new Alarm(new TimeSpan(0, 0, 10));
            alarm.IsBackground = false;
            alarm.AlarmOccured += Alarm_AlarmOccured;

            Console.WriteLine("Uruchomiono Punkt 3 (Kończenie v2 - IDisposable + Interrupt).");
            Console.WriteLine("Czekam na alarm lub nacisniecie dowolnego klawisza...");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }
            else
            {
                Thread.Sleep(1500);
            }

            Console.WriteLine("Wywolanie alarm.Dispose()...");
            alarm.Dispose();
            Thread.Sleep(200);
            Console.WriteLine("Koniec programu.");
        }

        private static void Alarm_AlarmOccured(object sender, AlarmEventArgs args)
        {
            Console.WriteLine($"{args.Message} at {args.AlarmFiredAt}");
        }
    }
}
