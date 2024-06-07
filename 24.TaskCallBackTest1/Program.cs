using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace _24.TaskCallBackTest1
{
    class Program
    {
        static void Main(string[] args)
        {         
            Func<int, int> handleFunc = HandleCallBackFunc;
            Func<int> funcInt = HandleCallBackInt;
            ////借助wait或者Await实际上是为了得到某个结果
            CallBackTest(HandleCallBack, handleFunc, funcInt).Wait();
            ////这里直接运行Runtimer方法实际上由于主线程退出了，也看不到结果
            Task.Run(RunTimer);

        }

        private static int HandleCallBackInt()
        {
            return 333;
        }

        private static int HandleCallBackFunc(int arg)
        {
            Console.WriteLine("arg is " + arg);
            return arg + 100;
        }

        private static void HandleCallBack(int obj, int obj2)
        {
            Console.WriteLine("total cycle" + obj.ToString());
        }
        ////实际使用时，Action用于输出Task中某字段的数值
        ////Func可以向Task输入某个数值，输出某个数值好像无用
        private static async Task CallBackTest(Action<int, int> callback, Func<int, int> callbackFunc, Func<int> callBackInt)
        {
            Console.WriteLine("Task Starts");
            await RunTimer();
            Console.WriteLine("Task Ends");

            callback?.Invoke(99, 10);
            callbackFunc?.Invoke(999);
            if (callBackInt != null)
            {
                int i = callBackInt.Invoke();
                Console.WriteLine("Input int " + i);
            }
            
        }

        private static async Task RunTimer()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"This is {i} cycle");
                await Task.Delay(200);
            }
        }
    }


}
