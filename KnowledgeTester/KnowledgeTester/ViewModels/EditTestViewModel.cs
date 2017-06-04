using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KnowledgeTester.BLL;
using KnowledgeTester.Model;

namespace KnowledgeTester.ViewModels
{
    internal class EditTestViewModel : ViewModelBase
    {
        public IUserService UserService { get; set; }
        public ITestService TestService { get; set; }

        public ICommand SaveTestCommand { get; private set; }

        public ICommand EditTaskCommand { get; private set; }
        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand AddTaskCommand { get; private set; }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                NotifyPropertyChanged();
            }
        }

        private Test _test;
        public Test Test
        {
            get { return _test; }
            set
            {
                _test = value;
                NotifyPropertyChanged();
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                NotifyPropertyChanged();
            }
        }

        private Question _newQuestion = new Question() {Id = Guid.NewGuid(), Number = 1};

        public EditTestViewModel()
        {

            AddTaskCommand = new Command((p) => FormsManager.Show(ViewsNames.CREATE_TASK, _newQuestion.Id, _newQuestion, View).Closing += EditTaskView_Closing);
            EditTaskCommand = new Command((p) => FormsManager.Show(ViewsNames.EDIT_TASK, SelectedQuestion.Id, SelectedQuestion, View));
            DeleteTaskCommand = new Command(OnDeleteTestCommand);
            SaveTestCommand = new Command(OnSaveTestCommand);
        }

        private void EditTaskView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var number = _newQuestion.Number;
            Test.Question.Add(_newQuestion);
            _newQuestion = new Question() {Id = Guid.NewGuid(), Number = number + 1};
            SynchronizeQuestionsCollections();
        }

        private void OnSaveTestCommand(object o)
        {
            //TODO: add changed by field to test
            var currentUser = UserService.GetCurrentUser();
            TestService.SaveTest(Test);
            View.Close();
        }

        private void OnDeleteTestCommand(object o)
        {
            Test.Question.Remove(SelectedQuestion);
            SynchronizeQuestionsCollections();
        }

        public override void Init(object data)
        {
            var test = data as Test;
            
            if (test != null)
            {
                TestService.GetTest(test.Id);
                Test = test;
            }

            else
            {
                var currentUser = UserService.GetCurrentUser();
                Test = new Test() {Id = Guid.NewGuid(), Question = new List<Question>(), Author = String.Format("{0} {1}", currentUser.Name, currentUser.LastName), Version  = 1};
            }
            Questions = new ObservableCollection<Question>();
            SynchronizeQuestionsCollections();
        }

        private void SynchronizeQuestionsCollections()
        {
            if (Questions.Any())
            {
                Questions.Clear();
            }

            foreach (var question in Test.Question)
            {
                Questions.Add(question);
            }
        }
    }
}
