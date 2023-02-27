using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace _1.DI_ServiceLocator_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServiceCollection serviceDescriptors = new ServiceCollection();
            ///服务类型是IService，实现类型是ServiceTest1
            serviceDescriptors.AddScoped<IService, ServiceTest1>();
            //serviceDescriptors.AddScoped(typeof(IService), typeof(ServiceTest1));
            using (ServiceProvider serviceProvider=serviceDescriptors.BuildServiceProvider())
            {
                IService service1 = serviceProvider.GetService<IService>();
                service1.Name = "qqq";
                service1.SayHi();
                //Console.WriteLine(service1.GetType());
                IEnumerable<IService> services=serviceProvider.GetServices<IService>();
                foreach (var item in services)
                {
                    Console.WriteLine(item.GetType());
                }

            }

            Console.ReadLine();
        }


    }

    public interface IService
    {
        string Name { get; set; }
        void SayHi();
    }

    public class ServiceTest1 : IService
    {
        public string Name { get; set; }
        public void SayHi()
        {
            Console.WriteLine($"1,Name is {Name}");
        }
    }

    public class ServiceTest2 : IService
    {
        public string Name { get; set; }
        public void SayHi()
        {
            Console.WriteLine($"2,Name is {Name}");
        }
    }
}
