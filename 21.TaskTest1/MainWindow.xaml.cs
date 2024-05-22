using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace _21.TaskTest1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isTaskRunning;
        public MainWindow()
        {
            InitializeComponent();
            isTaskRunning = false;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isTaskRunning)
            {
                MessageBox.Show("Task is already running!");
                return;
            }

            isTaskRunning = true;
            await RunTimerTask();
            isTaskRunning = false;
        }

        private async Task RunTimerTask()
        {
            for (int i = 0; i < 10; i++)
            {
                Text1.Dispatcher.Invoke(new Action(() => { Text1.Text = DateTime.Now.ToString(); }));
                await Task.Delay(1000);
                Debug.WriteLine($"Timer: {i + 1} seconds");
            }
            MessageBox.Show("Timer completed!");
        }
    }
}
