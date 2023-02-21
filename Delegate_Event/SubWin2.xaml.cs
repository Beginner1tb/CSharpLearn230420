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

namespace Delegate_Event
{
    /// <summary>
    /// SubWin2.xaml 的交互逻辑
    /// </summary>
    public partial class SubWin2 : Window
    {
        private static SubWin2 instance;
        public static object _lock = new object();
        public SubWin2()
        {
            InitializeComponent();
            UserEvent.UserProcesserEvent += Textbox_Modify;
            
        }

        public static SubWin2 GetInstance()
        {
            if (instance==null)
            {
                lock (_lock)
                {
                    instance = new SubWin2();
                }
            }
            return instance;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }

        private void Textbox_Modify(object sender,UserArgs e)
        {
            TB_Sub.Dispatcher.Invoke(() => { TB_Sub.Text = e.name; });
        }
    }
}
