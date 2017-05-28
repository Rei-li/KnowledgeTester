using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KnowledgeTester.BLL;
using KnowledgeTester.Model;

namespace KnowledgeTester.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }

        public string Login { set; get; }
        public string Password { set; get; }

        private bool _isLoginButtonVisible;
        public bool IsLoginButtonVisible
        {
            get { return _isLoginButtonVisible; }
            set
            {
                _isLoginButtonVisible = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isLogoutButtonVisible;
        public bool IsLogoutButtonVisible
        {
            get { return _isLogoutButtonVisible; }
            set
            {
                _isLogoutButtonVisible = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ExitCommand { get; private set; }
        public ICommand AddNewUserCommand { get; private set; }
        public ICommand AddNewTestCommand { get; private set; }

        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public MainWindowViewModel()
        {
            ExitCommand = new Command((p) => Application.Current.Shutdown());
            AddNewUserCommand = new Command((p) => FormsManager.Show(ViewsNames.REGISTER));
            AddNewTestCommand = new Command((p) => FormsManager.Show(ViewsNames.CREATE_TEST));

            LoginCommand = new Command(OnLoginCommand);
            LogoutCommand = new Command(OnLogoutCommand);
            
            IsLoginButtonVisible = true;
        }

        private void OnLoginCommand(object o)
        {
            CurrentUser = UserService.Login(Login, Password);
            IsLoginButtonVisible = (CurrentUser == null);
            IsLogoutButtonVisible = (CurrentUser != null);
        }

        private void OnLogoutCommand(object o)
        {
            CurrentUser = UserService.Logout();
            IsLoginButtonVisible = (CurrentUser == null);
            IsLogoutButtonVisible = (CurrentUser != null);
        }
        

    }
}
