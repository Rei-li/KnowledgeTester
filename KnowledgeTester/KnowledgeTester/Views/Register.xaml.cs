using System.Windows.Controls;
using KnowledgeTester.BLL;

namespace KnowledgeTester.Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : WindowViewBase
    {
        public IUserService UserService { get; set; }

        public Register()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserService.SaveUser();
        }
    }
}
