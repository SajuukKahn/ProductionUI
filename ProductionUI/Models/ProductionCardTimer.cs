using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ProductionUI.Models
{
    class ProductionCardTimer
    {
        private Task _timer;
        public Task Timer
        {
            get
            {
                return _timer;
            }
        }

        public ProductionCardTimer()
        {
            _timer = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                return .1;
            });
        }

        public void StartTimer()
        {
            _timer.Start();
        }
    }
}
