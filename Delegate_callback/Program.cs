using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate_callback
{
    //1、声明一个委托
    public delegate string Del_CallBack(string s);
    class Program
    {
        static void Main(string[] args)
        {
            //因为是在类的内部，所以必须实例化才能用
            Program program = new Program();
            //2、将用方法去实例化委托
            Del_CallBack delegate_Callback = program.Method1;
            //3、像普通方法一样调用委托
            Console.WriteLine(delegate_Callback("aaaaa"));

            //方法作为参数
            program.ActualMethod("1111", program.Method2);

            Func<string, string> func = new Func<string, string>(program.Method1);
            Action<int> action = new Action<int>(program.actionMethod);

            //下面两个调用过程等价，但是invoke调用可以判断是否为空
            action?.Invoke(123);
            action(456);
            //同理，func也一样
            var str1 = func.Invoke(",,,");
            var str2 = func("...");

            Console.ReadKey();
        }

        public string Method1(string s1)
        {
            var str = s1 + s1;
            return str;
        }

        public string Method2(string s2)
        {
            return s2 + "0";
        }

        //委托是封装好的方法，将委托以形参的形式发送出去
        public void ActualMethod(string s, Del_CallBack del_CallBack)
        {
            //实际上调用的是委托对应的方法
            Console.WriteLine(del_CallBack(s));

        }

        public void actionMethod(int i)
        {
            Console.WriteLine("actionMethod is " + i);
        }
    }


}
