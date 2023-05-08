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

namespace Wpf_ControlAdd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Hello, world!";
            textBlock.FontSize = 16;
            textBlock.Margin = new Thickness(20, 20, 30, 30);
            textBlock.Foreground = Brushes.Black;
            grid.Children.Add(textBlock);

            Button myButton = (Button)UserControl_1.FindName("Btn1");
            myButton.Content = "New Button Text";
            

        }

        private void UserControl_1_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
