using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KnowledgeTester.BLL;
using KnowledgeTester.Model;

namespace KnowledgeTester.ViewModels
{
    class EditTaskViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }
        public ITestService TestService { get; set; }

        public ICommand SaveTaskCommand { get; private set; }

        public ICommand EditAnswerCommand { get; private set; }
        public ICommand DeleteAnswerCommand { get; private set; }
        public ICommand AddAnswerCommand { get; private set; }

        private ObservableCollection<Answer> _answers;
        public ObservableCollection<Answer> Answers
        {
            get { return _answers; }
            set
            {
                _answers = value;
                NotifyPropertyChanged();
            }
        }

        private Question _question;
        public Question Question
        {
            get { return _question; }
            set
            {
                _question = value;
                NotifyPropertyChanged();
            }
        }

        private Answer _selectedAnswer;
        public Answer SelectedAnswer
        {
            get { return _selectedAnswer; }
            set
            {
                _selectedAnswer = value;
                NotifyPropertyChanged();
            }
        }

        private Answer _newAnswer = new Answer() { Id = Guid.NewGuid() };

        public EditTaskViewModel()
        {

            AddAnswerCommand = new Command((p) => FormsManager.Show(ViewsNames.CREATE_ANSWER, _newAnswer.Id, _newAnswer, View).Closing += EditTaskView_Closing);
            EditAnswerCommand = new Command((p) => FormsManager.Show(ViewsNames.EDIT_ANSWER, SelectedAnswer.Id, SelectedAnswer, View));
            DeleteAnswerCommand = new Command(OnDeleteAnswerCommand);
            SaveTaskCommand = new Command(OnSaveTaskCommand);
        }

        private void EditTaskView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Question.Answers.Add(_newAnswer);
            _newAnswer = new Answer{ Id = Guid.NewGuid() };
            SynchronizeAnswersCollections();
        }

        private void OnSaveTaskCommand(object o)
        {
            View.Close();
        }

        private void OnDeleteAnswerCommand(object o)
        {
            Question.Answers.Remove(SelectedAnswer);
            SynchronizeAnswersCollections();
        }


        public override void Init(object data)
        {
            var task = data as Question;
            if (task != null)
            {
                Question = task;
                if (Question.Answers == null)
                {
                    Question.Answers = new List<Answer>();
                }
                Answers = new ObservableCollection<Answer>();
                SynchronizeAnswersCollections();
            }

            else
            {
                View.Close();
            }
        }

        private void SynchronizeAnswersCollections()
        {
            if (Answers.Any())
            {
                Answers.Clear();
            }

            foreach (var answer in Question.Answers)
            {
                Answers.Add(answer);
            }
        }

    }
}
