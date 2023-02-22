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

namespace Event_Test1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //定义一个当前窗口的事件
        public TimeEventTest timeEventTest = new TimeEventTest();
        public MainWindow()
        {
            InitializeComponent();
            TimeTask();

            //事件的订阅，关联上关系，将Execute_Event方法传递给TimeTestProcessEvent事件处理器
            //timeEventTest.TimeTestProcessEvent += Execute_Event;

        }


        /// <summary>
        /// 事件发起者所在的任务，用于发送数据
        /// </summary>
        private void TimeTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    TextBlock.Dispatcher.Invoke(() =>
                    {
                        TextBlock.Text = DateTime.Now.ToString();
                        //启用事件
                        timeEventTest.SendToEventHandler(TextBlock.Text);
                    });
                    //不能写在非Dispatcher作用域外，不然不生效

                    await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// 响应事件，用于接收TimeTestArgs
        /// </summary>
        /// <param name="sender">发送对象，默认是null</param>
        /// <param name="timeTestArgs">接收参数值</param>
        public void Execute_Event(object sender, TimeTestArgs timeTestArgs)
        {
            //因为是特定的控件值改变，所以直接写控件名
            EventExecute_TextBlock.Dispatcher.Invoke(() =>
            {
                EventExecute_TextBlock.Text = timeTestArgs.currentTime;
            });
        }

        /// <summary>
        /// 定义事件数据的类
        /// </summary>
        public class TimeTestArgs : EventArgs
        {
            public string currentTime { get; set; }
        }

        /// <summary>
        /// 定义事件处理器的类
        /// </summary>
        public class TimeEventTest
        {
            /// <summary>
            /// 可以使用静态方法，关联事件响应处理
            /// </summary>
            public event EventHandler<TimeTestArgs> TimeTestProcessEvent;
            /// <summary>
            /// 可以使用静态方法，该方法把数据发给EventHandler，符合事件数据格式TimeTestArgs
            /// </summary>
            public void SendToEventHandler(string time)
            {
                TimeTestArgs timeTestArgs = new TimeTestArgs();
                timeTestArgs.currentTime = time;

                TimeTestProcessEvent?.Invoke(null, timeTestArgs);
            }

        }

        //可以随时订阅或者取消
        private void Btn_SubscribeEvent_Click(object sender, RoutedEventArgs e)
        {
            timeEventTest.TimeTestProcessEvent += Execute_Event;
        }

        private void Btn_CancelEvent_Click(object sender, RoutedEventArgs e)
        {
            timeEventTest.TimeTestProcessEvent -= Execute_Event;
        }
    }
}
