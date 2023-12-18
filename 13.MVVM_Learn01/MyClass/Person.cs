using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13.MVVM_Learn01.MyClass
{
    public class Person : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }

            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }



        private int age;

        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
