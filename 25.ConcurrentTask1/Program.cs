using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace _25.ConcurrentTask1
{
    class Program
    {
        static async Task Main(string[] args)
        {
           ConcurrentQueue<int> queue=new ConcurrentQueue<int>();
           CancellationTokenSource  cancellationTokenSource = new CancellationTokenSource();
           
           Task producerTask=Task.Run(async () =>
           {
               for (int i = 0; i < 10; i++)
               {
                   queue.Enqueue(i);
                   Console.WriteLine($"Produced: {i}");
                   await Task.Delay(1000);
               }
               cancellationTokenSource.Cancel();
           }, cancellationTokenSource.Token);
           Task consumerTask = Task.Run(async () =>
           {
               while (!queue.IsEmpty)
               {
                   if (queue.TryDequeue(out int item))
                   {
                       Console.WriteLine($"Consumed: {item}");
                       await Task.Delay(3000);
                   }
               }
           });
           await Task.WhenAll(producerTask, consumerTask);
           Console.WriteLine("Completed");
        }
    }
}
