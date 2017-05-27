using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KnowledgeTester.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region [ Свойства ]

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }

        public Window View { get; set; }

        #endregion

        #region [ IoC свойства ]

        //public ICommandQueryDispatcher Dispatcher { get; set; }

        //public INavigationManager NavigationManager { get; set; }

        public IFormsManager FormsManager { get; set; }

        //public IDataManager DataManager { get; set; }

        //public ISettingsManager SettingsManager { get; set; }

        //public IPrintManager PrintManager { get; set; }

        #endregion

        public virtual void OnNavigated()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Init(object data)
        {
            // Инифицализация
        }
    }
}
