using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _13.MVVM_Learn01.MyClass;
using GalaSoft.MvvmLight.Command;

namespace _13.MVVM_Learn01.ViewModel
{
    public class PersonViewModel:INotifyPropertyChanged
    {
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        public ICommand UpdateCommand { get; }

        public PersonViewModel()
        {
            Person = new Person { Name = "John Doe", Age = 30 };
            UpdateCommand = new RelayCommand(UpdateData);
        }

        private void UpdateData()
        {
            Person.Name = "Jane Smith";
            Person.Age = 35;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
