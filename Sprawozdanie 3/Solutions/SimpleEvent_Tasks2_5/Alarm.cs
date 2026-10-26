using System;
using System.Threading;

namespace SimpleEvent_Tasks2_5
{
    /// <summary>
    /// Аргументи події спрацьовування будильника (Alarm Occurred).
    /// </summary>
    public class AlarmEventArgs : EventArgs
    {
        public string Message { get; set; } = string.Empty;
        public DateTime AlarmFiredAt { get; set; }
    }

    /// <summary>
    /// Аргументи для нового тактового індикатора зворотного відліку (Zadanie 5).
    /// Передає точний залишок часу до запланованого сигналу.
    /// </summary>
    public class ClockTickEventArgs : EventArgs
    {
        public TimeSpan RemainingTime { get; set; }
        public int CheckingIntervalMs { get; set; }
    }

    /// <summary>
    /// Клас будильника Alarm.
    /// Реалізує інтерфейс IDisposable (Zadanie 3) для безпечного переривання фонового потоку.
    /// </summary>
    public class Alarm : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Поточний залишок часу до спрацьовування будильника (оновлюється в checkingTime).
        /// </summary>
        private TimeSpan timespanToAlarm;

        /// <summary>
        /// Абсолютний час, коли має пролунати сигнал будильника.
        /// </summary>
        private DateTime timeOfAlarm;

        /// <summary>
        /// Внутрішній робочий потік, що виконує періодичні перевірки часу.
        /// </summary>
        private Thread? internalThread;

        /// <summary>
        /// Інтервал перевірки умови часу в мілісекундах (Zadanie 4). За замовчуванням 1000 мс.
        /// </summary>
        private int checkingInterval;

        /// <summary>
        /// Прапорець для запобігання повторній утилізації ресурсів (IDisposable pattern).
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Режим роботи: чи використовувати IsBackground = true (Zadanie 2: v1),
        /// чи безпечне переривання через IDisposable.Interrupt() (Zadanie 3: v2).
        /// </summary>
        private readonly bool useBackgroundModeV1;
        #endregion

        #region Public Properties
        /// <summary>
        /// Властивість тільки для читання, що повертає поточний залишок часу (Zadanie 5).
        /// </summary>
        public TimeSpan TimespanToAlarm => timespanToAlarm;

        /// <summary>
        /// Інтервал перевірки часу (тільки для читання).
        /// </summary>
        public int CheckingInterval => checkingInterval;

        // Делегати та події
        public delegate void AlarmEventHandler(object sender, AlarmEventArgs args);

        /// <summary>
        /// Подія, що спрацьовує, коли настав встановлений час тривоги.
        /// </summary>
        public event AlarmEventHandler? AlarmOccured;

        /// <summary>
        /// Нова подія зворотного відліку (Zadanie 5), що виникає при кожній ітерації перевірки часу.
        /// </summary>
        public event EventHandler<ClockTickEventArgs>? ClockTicked;
        #endregion

        #region Constructors (Zadanie 4 - przeciążenia)
        /// <summary>
        /// Конструктор зі встановленням сигналу через відносний інтервал (TimeSpan)
        /// та можливістю передати власний checkingInterval (Zadanie 4).
        /// </summary>
        public Alarm(TimeSpan timespan, int checkingInterval = 1000, bool useBackgroundModeV1 = false)
        {
            this.checkingInterval = checkingInterval;
            this.useBackgroundModeV1 = useBackgroundModeV1;
            SetAlarm(timespan, checkingInterval);
        }

        /// <summary>
        /// Конструктор зі встановленням сигналу на точну дату і час (DateTime)
        /// та можливістю передати власний checkingInterval (Zadanie 4).
        /// </summary>
        public Alarm(DateTime at, int checkingInterval = 1000, bool useBackgroundModeV1 = false)
        {
            this.checkingInterval = checkingInterval;
            this.useBackgroundModeV1 = useBackgroundModeV1;
            SetAlarm(at, checkingInterval);
        }

        public Alarm(bool useBackgroundModeV1 = false)
        {
            this.checkingInterval = 1000;
            this.useBackgroundModeV1 = useBackgroundModeV1;
        }
        #endregion

        #region Public Configuration Methods (Zadanie 4)
        public void SetAlarm(TimeSpan timeSpan, int? interval = null)
        {
            if (interval.HasValue) checkingInterval = interval.Value;
            timespanToAlarm = timeSpan;
            timeOfAlarm = DateTime.Now.Add(timeSpan);
            Init();
        }

        public void SetAlarm(DateTime at, int? interval = null)
        {
            if (interval.HasValue) checkingInterval = interval.Value;
            timeOfAlarm = at;
            timespanToAlarm = at - DateTime.Now;
            if (timespanToAlarm.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(at), "Час будильника не може бути у минулому!");
            Init();
        }
        #endregion

        #region Private Engine Methods
        private void Init()
        {
            StartClock();
        }

        private void StartClock()
        {
            if (internalThread == null || !internalThread.IsAlive)
            {
                internalThread = new Thread(CheckingTime)
                {
                    Name = "AlarmMonitorThread"
                };

                // -----------------------------------------------------------------------------
                // ZADANIE 2: Kończenie v1
                // Якщо увімкнено режим v1, робимо потік фоновим (IsBackground = true).
                // У такому разі завершення головного потоку негайно вимикає потік CLR.
                // ZADANIE 3: Kończenie v2
                // За замовчуванням залишаємо IsBackground = false, а завершення виконуємо
                // коректно та безпечно через alarm.Dispose() -> Thread.Interrupt().
                // -----------------------------------------------------------------------------
                if (useBackgroundModeV1)
                {
                    internalThread.IsBackground = true;
                }
                else
                {
                    internalThread.IsBackground = false;
                }

                internalThread.Start(checkingInterval);
            }
        }

        /// <summary>
        /// Тіло робочого циклу моніторингу часу.
        /// </summary>
        private void CheckingTime(object? o)
        {
            int interval = (o is int i) ? i : 1000;

            try
            {
                while (DateTime.Now < timeOfAlarm)
                {
                    // -------------------------------------------------------------------------
                    // ZADANIE 5: Aktualizacja licznika i wyzwalanie nowego Eventa ClockTicked
                    // -------------------------------------------------------------------------
                    timespanToAlarm = timeOfAlarm - DateTime.Now;
                    if (timespanToAlarm.TotalMilliseconds < 0) timespanToAlarm = TimeSpan.Zero;

                    // Оповіщення всіх підписників про залишок часу
                    ClockTicked?.Invoke(this, new ClockTickEventArgs
                    {
                        RemainingTime = timespanToAlarm,
                        CheckingIntervalMs = interval
                    });

                    // Потік переходить у стан WaitSleepJoin.
                    // У цьому стані виклик internalThread.Interrupt() викличе ThreadInterruptedException.
                    Thread.Sleep(interval);
                }

                // Сигнал спрацював успішно
                timespanToAlarm = TimeSpan.Zero;
                AlarmEventArgs aea = new AlarmEventArgs
                {
                    Message = "Alarm was triggered! Час вичерпано!",
                    AlarmFiredAt = DateTime.Now
                };

                AlarmOccured?.Invoke(this, aea);
            }
            catch (ThreadInterruptedException)
            {
                // -----------------------------------------------------------------------------
                // ZADANIE 3: Kończenie v2 - Obsługa ThreadInterruptedException
                // Перехоплення переривання потоку: звільнення ресурсів та безпечне завершення.
                // -----------------------------------------------------------------------------
                Console.WriteLine("\n[WĄTEK ALARM] Otrzymano sygnał Thread.Interrupt(). Bezpieczne zwalnianie zasobów i zakończenie wątku potomnego.");
            }
        }
        #endregion

        #region IDisposable Implementation (Zadanie 3)
        /// <summary>
        /// Реалізація патерну IDisposable: викликає Interrupt() на робочому потоці та обнуляє посилання.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                if (internalThread != null && internalThread.IsAlive)
                {
                    // Перериваємо очікування в Thread.Sleep, що викликає ThreadInterruptedException всередині потоку
                    internalThread.Interrupt();
                    internalThread = null;
                }
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
