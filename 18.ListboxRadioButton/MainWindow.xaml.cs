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
using _18.ListboxRadioButton.Pages;

namespace _18.ListboxRadioButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Page1 page1 = new Page1();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LB1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FR1.Content = null;
            //var RB_button = LB1.SelectedItem as TextBlock;
            //MessageBox.Show(RB_button.Text.ToString());

            FR1.Content = new Page1();
           
        }
    }
}
