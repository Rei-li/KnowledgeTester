using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KnowledgeTester.BLL;

namespace KnowledgeTester.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }

        public string Name { set; get; }
        public string LastName { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }

        public ICommand AddUserCommand { get; private set; }

        public RegistrationViewModel()
        {
            CreateCommands();
        }

        private void CreateCommands()
        {
            AddUserCommand = new Command(OnAddUserCommand);
        }

        private void OnAddUserCommand(object o)
        {
            UserService.SaveUser(Name, LastName, Login, Password);
            View.Close();
        }

    }
}
