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
        //定义一个事件处理器，用于关联响应处理方法
        public static event EventHandler<UserArgs> UserProcesserEvent;
        //事件具体处理方法，数据应该如何发出去，并在最后调用事件
        //方法的参数要符合UserArgs的特点，是把形参里的数据经过一定处理转换成UserArgs
        public static void ProcessUser(string name)
        {
            UserArgs userArgs = new UserArgs();
            userArgs.name = name;

            //调用事件，和委托一个写法，实际上是通过ProcessUser将数据发给UserProcesserEvent关联的方法
            //一定要加判断，防止崩溃
            // UserProcesserEvent(null, userArgs);
            UserProcesserEvent?.Invoke(null, userArgs);
        }
    }
}
