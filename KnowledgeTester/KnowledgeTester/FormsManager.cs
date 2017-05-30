using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KnowledgeTester.Views;
using Spring.Context;

namespace KnowledgeTester
{
    /// <summary>
    /// Менеджер форм
    /// </summary>
    public class FormsManager : IFormsManager, IApplicationContextAware
    {
        private Dictionary<string, Window> _openWindows = new Dictionary<string, Window>();

        public Window Show(string viewName)
        {
           return Show(viewName, null, null, null);
        }

        public Window Show(string viewName, object id, object data, Window owner)
        {
            string viewId = string.Format("{0}_{1}", viewName, id);

            Window window = GetOpenedWindow(viewId);

            if (window == null)
            {
                object view = ApplicationContext.GetObject(viewName);

                if (view != null)
                {
                    window = view as Window;

                    if (window != null)
                    {
                        window.Owner = owner;

                        //bool isTaskBarView = (window as ITaskBarView) != null;
                        //window.ShowInTaskbar = isTaskBarView;
                        _openWindows.Add(viewId, window);
                    }
                }

                IInitView initView = view as IInitView;
                if (initView != null)
                {
                    initView.Init(data);
                }
            }

            if (window != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Show();

                if (!window.IsActive)
                {
                    window.Activate();
                }

                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }
            }
            return window;
        }

        public object ShowModal(string viewName, object data, Window owner)
        {
            object result = null;

            object view = ApplicationContext.GetObject(viewName);

            if (view != null)
            {
                Window window = view as Window;

                if (window != null)
                {
                    window.Owner = owner;

                    IInitView initView = view as IInitView;
                    if (initView != null)
                    {
                        initView.Init(data);
                    }

                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    //bool isTaskBarView = (window as ITaskBarView) != null;
                    //window.ShowInTaskbar = isTaskBarView;
                    window.ShowDialog();
                }
            }

            return result;
        }

        private Window GetOpenedWindow(string viewName)
        {
            Window window = null;
            if (_openWindows.ContainsKey(viewName))
            {
                window = _openWindows[viewName];

                bool isOpened = false;
                foreach (var window1 in Application.Current.Windows)
                {
                    if (window1 == window)
                    {
                        isOpened = true;
                        break;
                    }
                }

                if (!isOpened)
                {
                    _openWindows.Remove(viewName);
                    window = null;
                }
            }

            return window;
        }

        public IApplicationContext ApplicationContext { set; private get; }
    }
}
