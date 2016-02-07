using System;
using System.Windows.Input;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    /// <summary>
    ///  Defines a command in a ViewModel which can be easily bound to a View in a MVVM based scenario.
    /// </summary>
    /// <remarks></remarks>
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<object> myExecute;
        private readonly Func<bool> myCanExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            myExecute = execute;
            myCanExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return ((myCanExecute == null) ? true : myCanExecute());
        }

        public void Execute(object parameter)
        {
            myExecute(parameter);
        }
    }
}