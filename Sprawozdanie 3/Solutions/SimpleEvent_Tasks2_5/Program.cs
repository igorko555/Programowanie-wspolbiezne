using System;

namespace SimpleEvent_Tasks2_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==================================================================");
            Console.WriteLine("   LABORATORIUM 3 - ZADANIA 2-5: EVENTY, KOŃCZENIE WĄTKÓW I ALARM  ");
            Console.WriteLine("==================================================================\n");

            Console.WriteLine("Оберіть режим тестування завершення програми:");
            Console.WriteLine("1 - Zadanie 2: Kończenie v1 (Właściwość IsBackground = true)");
            Console.WriteLine("2 - Zadanie 3: Kończenie v2 (Interfejs IDisposable + Thread.Interrupt()) [ZALECANE]");
            Console.Write("Ваш вибір (натисніть 1 або 2, за замовчуванням 2): ");

            string? choice = Console.IsInputRedirected ? "2" : Console.ReadLine();
            bool useV1 = (choice?.Trim() == "1");

            Console.WriteLine($"\nОбрано: {(useV1 ? "Wariant v1 (IsBackground=true)" : "Wariant v2 (IDisposable + Interrupt)")}");

            // Встановлюємо будильник на 4 секунди з інтервалом перевірки 500 мс (Zadanie 4)
            TimeSpan duration = TimeSpan.FromSeconds(4);
            int intervalMs = 500;
            Console.WriteLine($"Ініціалізація Alarm: час = {duration.TotalSeconds}s, checkingInterval = {intervalMs}ms");

            using (Alarm alarm = new Alarm(duration, intervalMs, useBackgroundModeV1: useV1))
            {
                // Підписка на нову подію такту зворотного відліку (Zadanie 5)
                alarm.ClockTicked += (sender, e) =>
                {
                    Console.WriteLine($"[CLOCK TICK] Залишилося до сигналу: {e.RemainingTime.TotalSeconds:F1} сек (Властивість: {alarm.TimespanToAlarm.TotalSeconds:F1}s)");
                };

                // Підписка на подію спрацьовування будильника
                alarm.AlarmOccured += (sender, e) =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n>>> [ALARM OCCURED]: {e.Message} о {e.AlarmFiredAt:HH:mm:ss.fff} <<<");
                    Console.ResetColor();
                    Console.WriteLine("Натисніть ENTER для виходу з програми...");
                };

                Console.WriteLine("\n[ГОЛОВНИЙ ПОТІК] Очікування... Можете зачекати завершення часу АБО натиснути будь-яку клавішу зараз для переривання:");

                // Очікування натискання клавіші користувачем або тайм-аут у разі автоматичного тесту
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
                else
                {
                    // В автономному режимі емулюємо натискання клавіші через 2 секунди
                    Thread.Sleep(2100);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[ГОЛОВНИЙ ПОТІК] Отримано команду завершення! Викликаємо alarm.Dispose()...");
                Console.ResetColor();

                // ZADANIE 3: Виклик alarm.Dispose() для безпечного переривання потоку
                alarm.Dispose();

                // Невелика пауза, щоб побачити фінальне повідомлення переривання від потоку
                Thread.Sleep(200);
            }

            Console.WriteLine("\n[ГОЛОВНИЙ ПОТІК] Програма штатно завершила свою роботу.");
        }
    }
}
