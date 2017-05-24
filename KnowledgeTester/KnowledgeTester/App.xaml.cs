using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Spring.Context;
using Spring.Context.Support;

namespace KnowledgeTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);
            IApplicationContext context = ContextRegistry.GetContext();
            MainWindow = context.GetObject<MainWindow>();
            MainWindow.Show();
        }
    }
}
