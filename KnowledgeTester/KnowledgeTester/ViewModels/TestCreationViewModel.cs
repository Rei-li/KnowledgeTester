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
    class TestCreationViewModel : ViewModelBase
    {
        public ITestService TestService { get; set; }
        public IUserService UserService { get; set; }

        private string _theme;
        public string Theme {
            get { return _theme; }
            set
            {
                _theme = value;
                NotifyPropertyChanged();
            }
        }

        private string _qestionTask;
        public string QestionTask
        {
            get { return _qestionTask; }
            set
            {
                _qestionTask = value;
                NotifyPropertyChanged();
            }
        }

        private string _qestionNotes;
        public string QestionNotes
        {
            get { return _qestionNotes; }
            set
            {
                _qestionNotes = value;
                NotifyPropertyChanged();
            }
        }

        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isAnswerCorrect;
        public bool IsAnswerCorrect {
            get { return _isAnswerCorrect; }
            set
            {
                _isAnswerCorrect = value;
                NotifyPropertyChanged();
            }
        }

        private List<Answer> _answers = new List<Answer>();
        private List<Question> _questions = new List<Question>();

        public ICommand AddAnswerCommand { get; private set; }
        public ICommand AddTaskCommand { get; private set; }
        public ICommand SaveTestCommand { get; private set; }

        public TestCreationViewModel()
        {
            CreateCommands();
        }

        private void CreateCommands()
        {
            AddAnswerCommand = new Command(OnAddAnswerCommand);
            AddTaskCommand = new Command(OnAddTaskCommand);
            SaveTestCommand = new Command(OnSaveTestCommand);
        }

        private void OnAddAnswerCommand(object o)
        {
            _answers.Add(new Answer() {Id = Guid.NewGuid(), AnswerText = Answer , IsCorrect = IsAnswerCorrect });
            Answer = String.Empty;
            IsAnswerCorrect = false;
        }

        private void OnAddTaskCommand(object o)
        {
            var number = 1;
            if (_questions.Any())
            {
                number = _questions.Last().Number + 1;
            }
            _questions.Add(new Question() {Id = Guid.NewGuid(), Task = QestionTask, Notes = QestionNotes, Number = number, Answers = _answers});
            _answers = new List<Answer>();
            QestionTask = String.Empty;
            QestionNotes = String.Empty;
        }

        private void OnSaveTestCommand(object o)
        {
            var currentUser = UserService.GetCurrentUser();
            TestService.SaveTest(Theme, String.Format("{0} {1}", currentUser.Name, currentUser.LastName), _questions);
            View.Close();
        }
    }
}
