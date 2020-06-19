using System;

namespace ProductionUI.Models
{
    class ProductionCardTimer
    {

        private readonly System.Timers.Timer t;

        private DateTime _startTime;

        public ProductionCardTimer()
        {
            t = new System.Timers.Timer(1);
            t.Start();
        }

        public void ResetRunTime()
        {
            _startTime = System.DateTime.Now;
        }

        private TimeSpan Tick(object sender, EventArgs e)
        {
            return DateTime.Now - _startTime;
        }

        ~ProductionCardTimer()
        {
            t.Stop();
        }
    }
}
