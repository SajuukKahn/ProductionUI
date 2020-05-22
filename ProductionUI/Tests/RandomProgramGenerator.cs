using ProductionUI.Models;
using ProductionUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ProductionUI.Tests
{
    class RandomProgramGenerator
    {
        private Random random = new Random();

        ProcessContainerViewModel _pcm;

        public ProcessContainerViewModel GenerateRandom(ProcessContainerViewModel pcm)
        {
            _pcm = pcm;

            _pcm.ProcessLocation = ProcessLocation;
            _pcm.ProgramName = ProgramName;
            
            return _pcm;
        }

        private string ProcessLocation
        {
            get
            {
                return "Zone " + random.Next(1, 4).ToString();
            }
        }

        private string ProgramName
        {
            get
            {
                return "Program " + random.Next(1, 30).ToString();
            }
        }
    }
}
