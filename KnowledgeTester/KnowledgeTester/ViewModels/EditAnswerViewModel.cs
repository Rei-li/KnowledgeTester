using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KnowledgeTester.BLL;
using KnowledgeTester.Model;

namespace KnowledgeTester.ViewModels
{
    class EditAnswerViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }
        public ITestService TestService { get; set; }


        private Answer _answer;
        public Answer Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveAnswerCommand { get; private set; }

        public EditAnswerViewModel()
        {
            SaveAnswerCommand = new Command(OnSaveAnswerCommand);
        }

        private void OnSaveAnswerCommand(object o)
        {
            View.Close();
        }


        public override void Init(object data)
        {
            var answer = data as Answer;
            if (answer != null)
            {
                Answer = answer;
            }

            else
            {
                View.Close();
            }
        }
    }

}
