using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KnowledgeTester.Views
{
    public abstract class WindowViewBase : Window
    {
        //private ViewModelBase _viewModel;

        //public ViewModelBase ViewModel
        //{
        //    get { return _viewModel; }
        //    set
        //    {
        //        _viewModel = value;

        //        if (_viewModel != null)
        //        {
        //            _viewModel.View = this;
        //        }

        //        DataContext = _viewModel;
        //    }
        //}

        protected WindowViewBase()
        {
            Width = 1024;
            Height = 768;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach (Window window in this.OwnedWindows)
            {
                window.Close();
            }

            base.OnClosing(e);
        }
    }
}
