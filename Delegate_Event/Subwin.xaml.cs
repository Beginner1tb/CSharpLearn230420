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
    
    public partial class Subwin : Window
    {
        private static Subwin instance;
        private static object _lock = new object();
        public Subwin()
        {
            InitializeComponent();

        }

        public static Subwin GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    instance = new Subwin();
                }
            }
            return instance;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }

        private void TB_Sub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UserEvent.ProcessUser(TB_Sub.Text);
               
            }
        }
    }

    public class SubInvokeEvent
    {
        public event EventHandler<UserArgs> InvokeProcesserEvent;
        public void ProcessUser(string name)
        {
            UserArgs userArgs = new UserArgs();
            userArgs.name = name;

            InvokeProcesserEvent?.Invoke(null, userArgs);
        }
    }


}
