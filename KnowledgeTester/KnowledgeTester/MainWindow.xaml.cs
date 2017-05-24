using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KnowledgeTester.BLL;
using Spring.Context;
using Spring.Context.Support;

namespace KnowledgeTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IUserService UserService { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();
            //IApplicationContext ctx = ContextRegistry.GetContext("KnowledgeTester");
            //// Testing
            //UserService = (UserService)ctx.GetObject("aCardDeck");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UserService.SaveUser();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
