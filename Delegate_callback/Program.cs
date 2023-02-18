using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate_callback
{
    //1、声明一个委托
    public delegate string Del_CallBack(string s);
    //Action或者Func跟Delegate不同，虽然是类，但是只能视为delegate下的某种方法，
    //不能与delegate一样当类写，比如下面
    //public Action/Func xxxx;
    class Program
    {
        //func_Callback用于测试func回调
        public static Func<string, string> func_Callback;
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
            //BeginInvoke不能直接用了，需要注意
            Console.WriteLine("///////////////");



            //func_Callback用于测试func回调,因为main是静态的，所以定义的是static Func
            Console.WriteLine("测试func回调………………");
            func_Callback = program.Method1;
            program.FuncMethod("FuncMethod", func_Callback);

            Console.WriteLine("////////////////");
            Console.WriteLine("测试func1回调方法");
            //Func<string, string> func1 = new Func<string, string>(program.Method1);
            Func<string, string> func1 = program.Method1;
            program.Func1Method(func1);
            //匿名方法针对的是委托本身的实例化过程
            func1 = (string s) => { return (s + "12345"); };
            program.Func1Method(func1);
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
        //本质上是应用了特定的方法，假设1+1=2,再计算2+2=4时，可以把x+x方法通过委托发送出去
        public void ActualMethod(string s, Del_CallBack del_CallBack)
        {
            //实际上调用的是委托对应的方法
            Console.WriteLine(del_CallBack(s));

        }

        //Func形式的回调,可以看出，形参名称不是Func声明下的某类名，而就是Func<T>或者Action<T>
        public void FuncMethod(string s,Func<string,string> func)
        {
            Console.WriteLine(func(s));
        }

        public void actionMethod(int i)
        {
            Console.WriteLine("actionMethod is " + i);
        }

        public void Func1Method(Func<string,string> func)
        {
            Console.WriteLine(func("This is func1 test"));
        }

    }

    


}
