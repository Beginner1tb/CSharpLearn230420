using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;
using System.Windows.Controls;

namespace _4.Nlog_WpfTest
{
    class MyTextboxTarget : TargetWithLayout
    {
        private readonly TextBox _textBox;

        public MyTextboxTarget(TextBox textBox)
        {
            _textBox = textBox;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = this.Layout.Render(logEvent);
            _textBox.Dispatcher.Invoke(() => { _textBox.AppendText(message + Environment.NewLine); });
        }

    }
}
