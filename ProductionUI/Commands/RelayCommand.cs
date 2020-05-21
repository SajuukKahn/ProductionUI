using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProductionUI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// Constructor for relay command class, handles redirection of commands from the View to the ViewModel
        /// </summary>
        /// <param name="execute">Command to carry out</param>
        /// <param name="canExecute">Check for is allowed</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        /// <summary>
        /// Command management to exe
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// allows execution of a bound command
        /// </summary>
        /// <param name="parameter">Can Execute parameter</param>
        /// <returns>true/false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Runs the execution for the bound Command
        /// </summary>
        /// <param name="parameter">carries out execution</param>
        public void Execute(object parameter)
        {
            _execute(parameter ?? "<N/A>");
        }
    }
}
