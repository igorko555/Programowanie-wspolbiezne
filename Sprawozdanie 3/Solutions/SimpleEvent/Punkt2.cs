using System;

namespace SimpleEvent
{
    class Punkt2
    {
        static void Main(string[] args)
        {
            Alarm alarm = new Alarm(new TimeSpan(0, 0, 10));
            alarm.IsBackground = true;
            alarm.AlarmOccured += Alarm_AlarmOccured;

            Console.WriteLine("Uruchomiono Punkt 2 (Kończenie v1 - IsBackground = true).");
            Console.WriteLine("Czekam na alarm lub nacisniecie dowolnego klawisza...");

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey(true);
            }

            Console.WriteLine("Koniec programu.");
        }

        private static void Alarm_AlarmOccured(object sender, AlarmEventArgs args)
        {
            Console.WriteLine($"{args.Message} at {args.AlarmFiredAt}");
        }
    }
}
