using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BF6PowerSaver.Commands
{
    public abstract class CommandBase : ICommand
    {
        // Forward add/remove to CommandManager.RequerySuggested so WPF will re-query CanExecute automatically.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);

        // Request an immediate requery of all commands.
        protected void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
