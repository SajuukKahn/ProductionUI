using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ProductionUI.Models
{
    public class Alarm : INotifyPropertyChanged
    {
        private DispatcherTimer _timer = null;
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public Alarm()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _timer = new DispatcherTimer();
                _timer.Tick += timer_Tick;
                _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            }, DispatcherPriority.Normal);
        }

        private TimeSpan _initialDifference;
        private DateTime? _timeStarted;
        public DateTime? TimeStarted
        {
            get { return _timeStarted; }
            set
            {
                // If the value is new
                if (_timeStarted != value)
                {
                    // If timeStarted was previously null then start the new timer
                    if (!_timeStarted.HasValue)
                    {
                        _timeStarted = value;

                        // Get the initial difference between Now and TimeStarted
                        _initialDifference = DateTime.Now.Subtract(TimeStarted.Value);

                        //irolTimer = new System.Threading.Timer(TickTick, null, 1000, 1000);

                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            _stopWatch.Start();
                            _timer.Start();
                        }, DispatcherPriority.Normal);

                    }
                    // If the timeStarted had a value but now its gone (stop the timer)
                    else if (_timeStarted.HasValue && value == null)
                    {
                        if (_stopWatch.IsRunning)
                            _stopWatch.Stop();

                        _timeStarted = value;
                    }
                    // If we already have a timer going but for some reason we just received a different start time
                    else if (_timeStarted.HasValue && value != null)
                    {
                        _timeStarted = value;

                        // Change the initial difference
                        _initialDifference = DateTime.Now.Subtract(TimeStarted.Value);
                    }

                    OnPropertyChanged("TimeStarted");
                }
            }
        }

        private TimeSpan _timeElapsed = new TimeSpan();
        public TimeSpan TimeElapsed
        {
            get
            {
                return _timeElapsed;
            }
            set
            {
                _timeElapsed = value;
                OnPropertyChanged("TimeElapsed");
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_stopWatch.IsRunning)
            {
                TimeSpan elapsed = _stopWatch.Elapsed;
                TimeSpan total = _initialDifference + elapsed;

                TimeElapsed = total;
            }
        }

    }
}
