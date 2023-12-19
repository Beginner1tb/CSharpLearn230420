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
        private IMainView mainView;
        public ChildWindow(IMainView mView)
        {
            InitializeComponent();
            mainView = mView;
        }

        public void ShowImg(BitmapImage bitmapimage)
        {
            Img1.Source = bitmapimage;
        }

        //实现IChildView接口的方法
        public void UpdateContent(string content)
        {
            tb_child.Text = content;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainView.UpdateMain("22222");
        }
    }
}
