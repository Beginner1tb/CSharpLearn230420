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

        SubInvokeEvent subInvokeEvent = new SubInvokeEvent();
        //public delegate void pss(object sender,)
        public MainWindow()
        {
            InitializeComponent();
            DataBase dataBase = new DataBase();

            //UserEvent.UserProcesserEvent += dataBase.SaveToDb;
            //UserEvent.UserProcesserEvent += Btn_ShowModify;
            //UserEvent.UserProcesserEvent += Btn_CrossFormModify;

            
            subInvokeEvent.InvokeProcesserEvent += Btn_ShowModify;
        }

        private void Btn_Show_Click(object sender, RoutedEventArgs e)
        {
            Subwin subwin = Subwin.GetInstance();
            //Content实际上为button控件的属性，可以直接通过外部进行修改
            subwin.btn.Content = "12345";
            subwin.Show();
        }

        private void TB_Event_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                //UserEvent.ProcessUser(TB_Event.Text);
                subInvokeEvent.ProcessUser(TB_Event.Text);
            }
        }

        private void Btn_ShowModify(object sender,UserArgs e)
        {
            Btn_Show.Dispatcher.Invoke(() =>
            {
                Btn_Show.Content = e.name;
            });
        }

        public void Btn_CrossFormModify(object sender, UserArgs e)
        {
            Btn_CrossForm.Dispatcher.Invoke(() =>
            {
                Btn_CrossForm.Content = e.name;
            });
        }

        private void Btn_CrossForm_Click(object sender, RoutedEventArgs e)
        {
            SubWin2 subWin2 = SubWin2.GetInstance();
            subWin2.Show();
        }
    }
}
