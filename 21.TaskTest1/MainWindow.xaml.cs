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
        private Task t1;
        public MainWindow()
        {
            InitializeComponent();
            isTaskRunning = false;
            cancellationTokenSource = new CancellationTokenSource();

            //CancelTest1();
        }
        
        private async void CancelTest1()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            Task task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    // 检查取消请求
                    if (token.IsCancellationRequested)
                    {
                        Debug.WriteLine("任务取消请求已接收");
                        //注意，在debug模式下会直接抛出异常终止，运行模式下不会
                        token.ThrowIfCancellationRequested();
                    }

                    Debug.WriteLine("任务进行中...");
                    Thread.Sleep(1000); // 模拟工作
                }

                Debug.WriteLine("任务完成");
            }, token);

            await Task.Delay(3000);
            cts.Cancel();


            try
            {
                await task;
            }
            catch (Exception)
            {
                Debug.WriteLine("任务被取消");
            }
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
            t1 = RunTimerTask(cancellationTokenSource.Token);
            var taskStatusUpdateTask = UpdateTaskStatus(t1);
            try
            {
                await t1;
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Task was canceled!");
            }
            finally
            {
                isTaskRunning = false;
                await taskStatusUpdateTask;
            }


        }

        private async Task UpdateTaskStatus(Task task)
        {
            while (true)
            {
                var status = task.Status;
                var taskid = task.Id;
                Debug.WriteLine("TaskId: " + taskid.ToString() + "\nTaskstatus:" + status.ToString()); ;
                await Task.Delay(500);
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
                    await Task.Delay(1000);
                    await Task.Delay(1000, cancellationToken);

                    #region await测试
                    //try
                    //{
                    //    //await处抛出TaskCanceledException
                    //    await Task.Delay(1000, cancellationToken);
                    //}
                    //catch (Exception ex)
                    //{

                    //    MessageBox.Show("Canceled:   " + ex.Message);
                    //    //抛出异常后如果不跳出，则会继续执行
                    //    //return;
                    //}
                    #endregion
                    Debug.WriteLine($"Timer: {i + 1} seconds");

                }
                MessageBox.Show("Timer completed!");
            }
            ////1.因为Task任务在Cancel时候是立即结束，并抛出的是TaskCanceledException，所以拿到的是TaskCanceledException
            ////2.如果使用Task.Delay(1000),则在执行完1000ms延迟后再执行ThrowIfCancellationRequested，抛出的是OperationCanceledException
            ////3.注意，TaskCanceledException来自于OperationCanceledException,所以如果不catch，上面Button_Click都会catch到异常
            ////4.这里catch了Exception，所以Button_Click不会catch到异常
            ////5.同样的问题，如果在Task内部捕捉到了TaskCanceledException异常，则此时Task的状态会变成RanToCompletion，外部不能捕捉到Task的Cancel状态
            catch (OperationCanceledException ex)
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

        //功能与上述代码段相同
        private void SingleTask2_Click(object sender, RoutedEventArgs e)
        {
            if (isTaskRunning)
            {
                //
                Task task = Task.Run(() =>
                {
                    //注意try-catch要在Task内部，外部捕捉不到异常
                    try
                    {
                        // 模拟长时间运行的任务
                        for (int i = 0; i < 10; i++)
                        {
                            // 检查取消请求
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                Console.WriteLine("任务被取消");
                                //需要通过ThrowIfCancellationRequested()抛出异常，否则不会停止
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                            }

                            Debug.WriteLine("任务进行中...");
                            Thread.Sleep(1000); // 模拟工作
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    Debug.WriteLine("任务完成");
                }, cancellationTokenSource.Token);


            }
        }
    }
}
