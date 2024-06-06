using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _23.TaskFactotyTest1
{
    public static class TaskExtensions
    {
        ////使用void是不合适用法
        ////最终还是回到Task
        ////void存在的问题是无法等待结束,并且就算应用同步等待.Wait方法，还是得用Task
        ////使用void进行的任务，在其所在的原线程（如主线程）结束后就结束，不一定会执行完毕
        public async static Task YesAwait(this Task task,Action<Exception> errorCallback)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {

                errorCallback?.Invoke(ex);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            //program.InvokeTask();
            ////如果用await，主函数返回属性改成async Task
            program.DoSomething().YesAwait(HandleError).Wait();
            //await program.AwaitTask();
            //int i = 0;
        }

        private static void HandleError(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        async Task DoSomething()
        {
            Console.WriteLine("Yes");
            await Task.Delay(3000);
           
            throw new Exception("Task Exception");
        }



        async void InvokeTask()
        {
            await AwaitTask();
        }

        private async Task AwaitTask()
        {
            var tasks = new List<Task>();

            
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(RunAsyncTasks(i));
            }

            await Task.WhenAll(tasks);


            Console.WriteLine("Tasks all completed");

            
        }

        private async Task RunAsyncTasks(int i)
        {
            Random random = new Random();
            
            int taskId = Task.CurrentId ?? random.Next(9999);
            Console.WriteLine($"Task {i} with ID {taskId} is running.");      
            await Task.Delay(1000);
            Console.WriteLine($"Task {i} with ID {taskId} is completed.");
        }
    }
}
