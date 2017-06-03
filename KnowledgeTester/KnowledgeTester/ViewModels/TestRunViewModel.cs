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
    class TestRunViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }

        private Test _test;
        private List<UserAnswer> _userAnswers;

        public string Theme { get; set; }

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

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get { return _isNextButtonVisible; }
            set
            {
                _isNextButtonVisible = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isSaveButtonVisible;
        public bool IsSaveButtonVisible
        {
            get { return _isSaveButtonVisible; }
            set
            {
                _isSaveButtonVisible = value;
                NotifyPropertyChanged();
            }
        }


        public ICommand NextCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public TestRunViewModel()
        {
            CreateCommands();
        }

        private void SetQuestion(int number)
        {
            try
            {
                Question = _test.Question.SingleOrDefault(s => s.Number.Equals(number));
                SetQestionAnswers();
            }
            catch (Exception)
            {
                //TODO: add proper error handling
                View.Close();
            }
        }

        private void SetQestionAnswers()
        {
            Answers = new ObservableCollection<Answer>();
            if (Question != null)
            {
                foreach (var answer in Question.Answers)
                {
                    Answers.Add(answer);
                }
            }
        }

        private void CreateCommands()
        {
            NextCommand = new Command(OnNextCommand);
            SaveCommand = new Command(OnSaveCommand);
        }

        private void OnNextCommand(object o)
        {
            _userAnswers.Add(new UserAnswer() { Id = Guid.NewGuid(), Answer = _selectedAnswer, QuestionNumber = Question.Number });
            int number = Question.Number + 1;
            SetQuestion(number);
            if (_test.Question.SingleOrDefault(s => s.Number.Equals(number + 1)) != null)
            {
                IsNextButtonVisible = true;
                IsSaveButtonVisible = false;
            }
            else
            {
                IsNextButtonVisible = false;
                IsSaveButtonVisible = true;
            }
        }

        private void OnSaveCommand(object o)
        {
            _userAnswers.Add(new UserAnswer() { Id = Guid.NewGuid(), Answer = _selectedAnswer, QuestionNumber = Question.Number });
            UserService.SaveUserTest(_test, _userAnswers);
            View.Close();
        }

        public override void Init(object data)
        {
            var test = data as Test;
            if (test != null)
            {
                _test = test;
                _userAnswers = new List<UserAnswer>();
                Theme = _test.Theme;
                int number = 1;
                SetQuestion(number);

                if (_test.Question.SingleOrDefault(s => s.Number.Equals(number + 1)) != null)
                {
                    IsNextButtonVisible = true;
                    IsSaveButtonVisible = false;
                }
                else
                {
                    IsNextButtonVisible = false;
                    IsSaveButtonVisible = true;
                }
            }
            else
            {
                View.Close();
            }
        }
    }
}
