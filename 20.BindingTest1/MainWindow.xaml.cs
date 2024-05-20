using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace _20.BindingTest1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CompoundClass CompoundClass { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            CompoundClass = new CompoundClass();
            CompoundClass.StringArrayWrapper = new StringArrayWrapper();
            CompoundClass.StrTestClass = new StrTestClass();
            this.DataContext = CompoundClass;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CompoundClass.StringArrayWrapper.Strings[1] = "222";
            CompoundClass.StrTestClass.TestStr = "other string";
        }
    }

    public class StringArrayWrapper : INotifyPropertyChanged
    {
        private ObservableCollection<string> _strings;

        public event PropertyChangedEventHandler PropertyChanged;

        public StringArrayWrapper()
        {
            _strings = new ObservableCollection<string> { "First", "Second", "Third" };
        }

        public ObservableCollection<string> Strings
        {
            get { return _strings; }
            set
            {
                if (_strings != value)
                {
                    _strings = value;
                    OnPropertyChanged(nameof(Strings));
                }
            }
        }

        //索引器
        //public string this[int index]
        //{
        //    get { return _strings[index]; }
        //    set
        //    {
        //        if (_strings[index] != value)
        //        {
        //            _strings[index] = value;
        //            OnPropertyChanged($"Strings[{index}]"); // 改为Strings[{index}]
        //        }
        //    }
        //}

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class StrTestClass : INotifyPropertyChanged
    {
        private string _testStr;

        public string TestStr
        {
            get { return _testStr; }
            set
            {
                if (_testStr!=value)
                {
                    _testStr = value;
                    OnPropertyChanged(nameof(TestStr));
                } }
        }
        public StrTestClass()
        {
            TestStr = "Initial String";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class CompoundClass : INotifyPropertyChanged
    {
        public StringArrayWrapper StringArrayWrapper { get; set; }
        public StrTestClass StrTestClass { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
