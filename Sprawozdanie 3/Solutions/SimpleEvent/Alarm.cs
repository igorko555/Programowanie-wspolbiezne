using System;
using System.Threading;

namespace SimpleEvent
{
    public class AlarmEventArgs : EventArgs
    {
        public string Message { get; set; } = string.Empty;
        public DateTime AlarmFiredAt { get; set; }
    }

    public class ClockTickEventArgs : EventArgs
    {
        public TimeSpan RemainingTime { get; set; }
        public int CheckingIntervalMs { get; set; }
    }

    public class Alarm : IDisposable
    {
        private TimeSpan timespanToAlarm;
        private DateTime timeOfAlarm;
        private Thread? internalThread;
        private int checkingInterval = 1000;
        private bool disposed = false;

        public bool IsBackground { get; set; } = false;
        public TimeSpan TimespanToAlarm => timespanToAlarm;
        public int CheckingInterval => checkingInterval;

        public delegate void AlarmEventHandler(object sender, AlarmEventArgs args);
        public event AlarmEventHandler? AlarmOccured;
        public event EventHandler<ClockTickEventArgs>? ClockTicked;

        public Alarm(TimeSpan timespan, int checkingInterval = 1000)
        {
            this.checkingInterval = checkingInterval;
            SetAlarm(timespan, checkingInterval);
        }

        public Alarm(DateTime at, int checkingInterval = 1000)
        {
            this.checkingInterval = checkingInterval;
            SetAlarm(at, checkingInterval);
        }

        public Alarm()
        {
            this.checkingInterval = 1000;
        }

        public void SetAlarm(TimeSpan timeSpan, int? interval = null)
        {
            if (interval.HasValue) checkingInterval = interval.Value;
            timespanToAlarm = timeSpan;
            timeOfAlarm = DateTime.Now.Add(timeSpan);
            init();
        }

        public void SetAlarm(DateTime at, int? interval = null)
        {
            if (interval.HasValue) checkingInterval = interval.Value;
            timeOfAlarm = at;
            timespanToAlarm = at - DateTime.Now;
            if (timespanToAlarm.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(at), "Datetime can't be from past");
            init();
        }

        private void init()
        {
            startClock();
        }

        private void startClock()
        {
            if (internalThread == null || !internalThread.IsAlive)
            {
                internalThread = new Thread(checkingTime);
                internalThread.IsBackground = IsBackground;
                internalThread.Start(checkingInterval);
            }
        }

        private void checkingTime(object? o)
        {
            int interval = (o is int i) ? i : 1000;

            try
            {
                while (DateTime.Now < timeOfAlarm)
                {
                    timespanToAlarm = timeOfAlarm - DateTime.Now;
                    if (timespanToAlarm.TotalMilliseconds < 0) timespanToAlarm = TimeSpan.Zero;

                    ClockTicked?.Invoke(this, new ClockTickEventArgs
                    {
                        RemainingTime = timespanToAlarm,
                        CheckingIntervalMs = interval
                    });

                    Thread.Sleep(interval);
                }

                timespanToAlarm = TimeSpan.Zero;
                AlarmEventArgs aea = new AlarmEventArgs
                {
                    Message = "Alarm was triggered",
                    AlarmFiredAt = DateTime.Now
                };

                AlarmOccured?.Invoke(this, aea);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Watek zakonczony przez Interrupt (ThreadInterruptedException).");
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (internalThread != null && internalThread.IsAlive)
                {
                    internalThread.Interrupt();
                    internalThread = null;
                }
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
