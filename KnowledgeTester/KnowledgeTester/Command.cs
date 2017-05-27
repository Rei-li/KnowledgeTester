using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnowledgeTester
{
    public class Command : ICommand
    {
        protected Action<object> OnExecute { get; private set; }

        protected Func<object, bool> OnCanExecute { get; private set; }

        public Command(Action<object> onExecute)
            : this(onExecute, (p) => true)
        {
        }

        public Command(Action<object> onExecute, Func<object, bool> onCanExecute)
        {
            OnExecute = onExecute;
            OnCanExecute = onCanExecute;
        }

        public void Execute(object parameter)
        {
            if (OnExecute != null)
            {
                OnExecute(parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            return OnCanExecute != null && OnCanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void NotifyCanExecuteChanged()
        {
            EventHandler canExecuteChanged = CanExecuteChanged;
            if (canExecuteChanged != null)
            {
                canExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
