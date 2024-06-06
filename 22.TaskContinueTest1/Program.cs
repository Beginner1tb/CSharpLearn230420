using System;
using System.Threading.Tasks;

namespace _22.TaskContinueTest1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ////Task.wait()方法会阻塞主线程，必须等执行完，尽量不使用
            ////可以不用改变程序段的返回值
            ////如果中途打断可能引起死锁问题
            ////尽量使用Task中的async/await方法，如果使用async/await，返回值必须为Task
            //ExecuteTasksInSeries().Wait();

            await ExecuteTasksInSeries();
            await ExecuteTasksInSeries2();
        }

        static Task ExecuteTasksInSeries()
        {
            // 定义任务
            Task task1 = Task1();

            // 使用 ContinueWith 方法串行执行任务
            var taskkk = task1.ContinueWith(t => Task2());
            Task task2 = task1.ContinueWith(t => Task2()).Unwrap();
            Task task3 = task2.ContinueWith(t => Task3()).Unwrap();

            // 返回最终任务，以便在 Main 方法中等待所有任务完成
            return task3;
        }

        static Task Task1()
        {
            return Task.Delay(1000).ContinueWith(t => Console.WriteLine("Task 1 completed"));
        }

        static Task Task2()
        {
            return Task.Delay(1000).ContinueWith(t => Console.WriteLine("Task 2 completed"));
        }

        static Task Task3()
        {
            return Task.Delay(1000).ContinueWith(t => Console.WriteLine("Task 3 completed"));
        }

        static async Task ExecuteTasksInSeries2()
        {
            await Task4();
            await Task5();
            await Task6();
        }

        static async Task Task4()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task 4 completed");
        }
        static async Task Task5()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task 5 completed");
        }
        static async Task Task6()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task 6 completed");
        }
    }
}
