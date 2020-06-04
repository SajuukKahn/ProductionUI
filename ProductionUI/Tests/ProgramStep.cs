namespace ProductionUI.Tests
{
    using ProductionUI.ViewModels;
    using System;
    using System.Threading;

    public class ProgramStep
    {
        private double _executionTime;
        private int _numberOfIndices;
        private string _stepName;
        private string _displayMessage;
        private ProductionCardDeckViewModel _vm;

        public ProgramStep(ProductionCardDeckViewModel vm, int numberofindices, string stepName = "a", string displayMessage = "b")
        {
            // Generate a random execution time between 0 and 3 seconds.
            _executionTime = new Random().Next(0, 4) + new Random().NextDouble();
            _stepName = stepName;
            _displayMessage = displayMessage;
            _numberOfIndices = numberofindices;
            _vm = vm;

        }

        public string StepName
        {
            get
            {
                return _stepName;
            }
        }

        public string DisplayMessage
        {
            get
            {
                return _displayMessage;
            }
        }

        public virtual void Run()
        {
            // Wait _executionTimeQ
            for (int i = 0; i < _numberOfIndices; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(0, 4) + new Random().NextDouble()));
                _vm.NextCard("Step Data Log Passed Parameter" + i.ToString());
            }
        }
    }
}
