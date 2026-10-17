using System;

namespace SimpleEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            Alarm alarm = new Alarm(new TimeSpan(0, 0, 5));
            alarm.AlarmOccured += Alarm_AlarmOccured;
            Console.WriteLine("Now I'm waiting for alarm or key press...");
            Console.ReadKey(true); //true to avoid displaying letter
          
        }

        private static void Alarm_AlarmOccured(object sender, AlarmEventArgs args)
        {
            Console.WriteLine($"We got it! {args.Message} at {args.AlarmFiredAt}");
        }
    }
}
