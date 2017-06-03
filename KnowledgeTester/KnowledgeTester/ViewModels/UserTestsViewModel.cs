using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.BLL;
using KnowledgeTester.Model;

namespace KnowledgeTester.ViewModels
{
    class UserTestsViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }

        private ObservableCollection<UserTest> _userTests;
        public ObservableCollection<UserTest> UserTests
        {
            get { return _userTests; }
            set
            {
                _userTests = value;
                NotifyPropertyChanged();
            }
        }

        public  void Init()
        {
            var currentUser = UserService.GetCurrentUser();
            if (currentUser != null)
            {
                UserTests = new ObservableCollection<UserTest>();
                foreach (var userTest in currentUser.TakenTests)
                {
                    UserTests.Add(userTest);
                }
            }
            else
            {
                View.Close();
            }
        }
    }
}
