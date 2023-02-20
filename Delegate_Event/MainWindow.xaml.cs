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

namespace Delegate_Event
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //public delegate void pss(object sender,)
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Btn_Show_Click(object sender, RoutedEventArgs e)
        {
            Subwin subwin = Subwin.GetInstance();
            subwin.btn.Content = "12345";
            subwin.Show();
        }
    }
}
