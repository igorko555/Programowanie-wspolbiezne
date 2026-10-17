using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimpleEvent
{
    /// <summary>
    /// Class used to pass Event parameters
    /// </summary>
    public class AlarmEventArgs : EventArgs
    {
        public string Message { get; set; }
        public DateTime AlarmFiredAt { get; set; }
    }
    public class Alarm 
    {
        #region private variable
        /// <summary>
        /// Time remain to fire up alarm
        /// </summary>
        TimeSpan timespanToAlarm;
        /// <summary>
        /// Time point when alarm wil be fired up
        /// </summary>
        DateTime timeOfAlarm;
        /// <summary>
        /// Thread checking whether to trigger an event
        /// </summary>
        Thread internalTrhead;
        /// <summary>
        /// How often in ms thread sholud check
        /// </summary>
        int checkingInterval;
        #endregion

        #region public properties
        public delegate void AlarmEventHandler(object sender, AlarmEventArgs args);
        /// <summary>
        ///This Event is fired up when time condition is fulfilled
        /// </summary>
        public event AlarmEventHandler AlarmOccured;
        #endregion

        #region constructors
        /// <summary>
        /// Constructor with setting alarm at NOW + timespan
        /// </summary>
        /// <param name="timespan"></param>
        public Alarm(TimeSpan timespan)
        {
            SetAlarm(timespan);
        }

        /// <summary>
        /// Constructor with setting alarm at 'at'
        /// </summary>
        /// <param name="at"></param>
        public Alarm(DateTime at)
        {
            SetAlarm(at);
        }

        public Alarm()
        {

        }
        #endregion

        #region private methods
        /// <summary>
        /// Init method, it set the default chekingInterval and start Clock.
        /// </summary>
        private void init()
        {
            checkingInterval = 1000;
            startClock();
        }

         

        private void startClock()
        {
            if (internalTrhead == null || !internalTrhead.IsAlive)
            {
                internalTrhead = new Thread(checkingTime);
                internalTrhead.Start(checkingInterval);
            }
        }
        private void checkingTime(object o)
        {          
                int interval = (int)o;
                while (DateTime.Now < timeOfAlarm)
                {
                    Thread.Sleep(interval); //interval between checks
                }
                AlarmEventArgs aea = new AlarmEventArgs();
                aea.Message = "Alarm was triggered";
                aea.AlarmFiredAt = DateTime.Now;
                if (AlarmOccured != null) AlarmOccured(this, aea);
         }
        #endregion

        #region public methods
        public void SetAlarm(TimeSpan timeSpan)
        {
            timespanToAlarm = timeSpan;
            timeOfAlarm = DateTime.Now.Add(timeSpan);
            init();
        }

        public void SetAlarm(DateTime at)
        {
            timeOfAlarm = at;
            timespanToAlarm = at-DateTime.Now;
            if (timespanToAlarm.TotalMilliseconds < 0) throw new ArgumentOutOfRangeException("Datetime can't be from past");
            init();
        }

        #endregion
             
    }
}
