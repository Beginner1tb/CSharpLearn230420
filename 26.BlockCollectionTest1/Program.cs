using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace _26.BlockCollectionTest1
{
    class Program
    {
        static void Main(string[] args)
        {

            BlockingCollection<int> queue = new BlockingCollection<int>();
            
            Task producerTask = Task.Run(async () =>
            {
                int i = 0;
                while (true)
                {
                    queue.Add(i);
                    Console.WriteLine($"Produced: {i}");
                    i++;
                    await Task.Delay(1000);
                }

            });
            //监控producerTask是否结束，结束则重启producerTask任务
           
            Task consumerTask = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3000);
                    if (queue.TryTake(out int item))
                    {
                        Console.WriteLine($"Consumed: {item}");
                    }
                    else
                    {
                        break;
                    }
                }
            });
            Task.WaitAll(producerTask, consumerTask);
            Console.WriteLine("Completed");

        }
    }
}
