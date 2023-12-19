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
using System.Windows.Shapes;
using _17.InterfaceChildView.IDepository;

namespace _17.InterfaceChildView
{
    /// <summary>
    /// ChildWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChildWindow : Window,IChildView
    {
       
        public ChildWindow()
        {
            InitializeComponent();
            
        }

        public void ShowImg(BitmapImage bitmapimage)
        {
            Img1.Source = bitmapimage;
        }

        public void UpdateContent(string content)
        {
            tb_child.Text = content;
        }
    }
}
