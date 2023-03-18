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
using System.Reflection;

namespace _5.Reflection_Controls
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime dt = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            //Type type = Btn.GetType();
            //PropertyInfo propertyInfo = type.GetProperty("Content");
            //propertyInfo.SetValue(Btn, "1111");

            //System.Windows.Controls.Label label = new Label();
            //label.Content = "label";
            //label.Margin = new Thickness(0.1, 0.1, 0, 0);
            //MainGrid.Children.Add(label);
            
            
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateT = DateTime.Now;
            TimeSpan timeSpan = dateT - dt;
            Console.WriteLine(timeSpan.TotalMilliseconds.ToString()); ;
        }
    }
}
