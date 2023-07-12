using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13.MVVM_Learn01.ViewModel
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private string myProperty;
        public string MyProperty
        {
            get { return myProperty; }
            set
            {
                myProperty = value;
                OnPropertyChanged(nameof(MyProperty));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
