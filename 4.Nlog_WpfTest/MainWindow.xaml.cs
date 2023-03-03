using System.Windows;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace _4.Nlog_WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public MainWindow()
        {
            InitializeComponent();
            ///新加规则，则原有的规则无效
            //var config = new LoggingConfiguration();

            ///从原有的nlog.config中读取规则
            var config = new XmlLoggingConfiguration("nlog.config");
            var textboxTarget = new MyTextboxTarget(MyTextbox);
            config.AddTarget("mytextbox", textboxTarget);
            LoggingRule rule = new LoggingRule("*", LogLevel.Trace, textboxTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("222222");
        }
    }
}
