using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource cancellationTokenSource;
        public MainWindow()
        {
            InitializeComponent();
            isTaskRunning = false;
            cancellationTokenSource = new CancellationTokenSource();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isTaskRunning)
            {
                MessageBox.Show("Task is already running!");
                return;
            }

            isTaskRunning = true;

            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await RunTimerTask(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Task was canceled!");
            }
            finally
            {
                isTaskRunning = false;
            }
        }

        private async Task RunTimerTask(CancellationToken cancellationToken)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    Text1.Dispatcher.Invoke(new Action(() => { Text1.Text = DateTime.Now.ToString(); }));
                    cancellationToken.ThrowIfCancellationRequested();
                    //await Task.Delay(1000);
                    await Task.Delay(1000, cancellationToken);

                    #region await测试
                    ////try
                    ////{
                    ////    //await处抛出TaskCanceledException
                    ////    await Task.Delay(1000, cancellationToken);
                    ////}
                    ////catch (Exception ex)
                    ////{

                    ////    MessageBox.Show("Canceled:   " + ex.Message);
                    ////    //抛出异常后如果不跳出，则会继续执行
                    ////    //return;
                    ////}
                    #endregion
                    Debug.WriteLine($"Timer: {i + 1} seconds");
                }
                MessageBox.Show("Timer completed!");
            }
            ////1.因为Task任务在Cancel时候是立即结束，并抛出的是TaskCanceledException，所以拿到的是TaskCanceledException
            ////2.如果使用Task.Delay(1000),则在执行完1000ms延迟后再执行ThrowIfCancellationRequested，抛出的是OperationCanceledException
            ////3.注意，TaskCanceledException来自于OperationCanceledException,所以如果不catch，上面Button_Click都会catch到异常
            ////4.这里catch了Exception，所以Button_Click不会catch到异常
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetType().Name.ToString());
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (isTaskRunning && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                
            }
        }
    }
}
