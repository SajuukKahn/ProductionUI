using ProductionUI.Abstracts;
using ProductionUI.Commands;
using ProductionUI.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ProductionUI.ViewModels
{
    public class ActiveProductionCardViewModel : AProductionCard
    {
        DispatcherTimer t;

        private DateTime _startTime = System.DateTime.Now;


        public ActiveProductionCardViewModel()
        {
            PlayButton = new RelayCommand(o => PlayPressed());
            PauseButton = new RelayCommand(o => PausePressed());

            DependencyObject dep = new DependencyObject();
            if (DesignerProperties.GetIsInDesignMode(dep) != true)
            {
                t = new DispatcherTimer();
                t.IsEnabled = true;
                t.Interval = TimeSpan.FromMilliseconds(1);
                t.Tick += Tick;
                ResetRunTime();
                t.Start();
            }
        }

        ~ActiveProductionCardViewModel()
        {
            t.Stop();
            t.IsEnabled = false;
        }


        public void ResetRunTime()
        {
            _startTime = System.DateTime.Now;
        }

        private void Tick(object sender, EventArgs e)
        {
            StepActiveTime = Convert.ToString(DateTime.Now - _startTime);
        }

        private void PausePressed()
        {
            throw new NotImplementedException();
        }

        private void PlayPressed()
        {
            throw new NotImplementedException();
        }

    }

}
