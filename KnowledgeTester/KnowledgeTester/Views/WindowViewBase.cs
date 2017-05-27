using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KnowledgeTester.ViewModels;

namespace KnowledgeTester.Views
{
    public abstract class WindowViewBase : Window
    {
        private ViewModelBase _viewModel;

        public ViewModelBase ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;

                if (_viewModel != null)
                {
                    _viewModel.View = this;
                }

                DataContext = _viewModel;
            }
        }

        protected WindowViewBase()
        {
            Width = 525;
            Height = 350;
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
