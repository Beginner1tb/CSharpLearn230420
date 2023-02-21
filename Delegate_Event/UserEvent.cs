using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate_Event
{
    //事实上等同于设定一个静态变量用于全局的事件处理
    public class UserEvent
    {
        public static event EventHandler<UserArgs> UserProcesserEvent;

        public static void ProcessUser(string name)
        {
            UserArgs userArgs = new UserArgs();
            userArgs.name = name;

            //调用事件，和委托一个写法
            //一定要加判断，防止崩溃
            // UserProcesserEvent(null, userArgs);
            UserProcesserEvent?.Invoke(null, userArgs);
        }
    }
}
