using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnowledgeTester.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ExitCommand { get; private set; }
        public ICommand AddNewUserCommand { get; private set; }

        public MainWindowViewModel()
        {
            ExitCommand = new Command((p) => Application.Current.Shutdown());
            AddNewUserCommand = new Command((p) => FormsManager.Show("Register"));
        }
    }
}
