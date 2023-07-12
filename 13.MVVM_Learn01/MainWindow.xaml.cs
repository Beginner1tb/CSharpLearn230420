using _13.MVVM_Learn01.ViewModel;
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

namespace _13.MVVM_Learn01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MainViewModel mainViewModel = (MainViewModel)DataContext;
            //mainViewModel.MyProperty = DateTime.Now.ToString("ss:ffff");
            PersonViewModel personViewModel = (PersonViewModel)DataContext;
            personViewModel.Person.Age = 9999;
            

            
        }
    }
}
