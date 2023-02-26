using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection_Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            ///使用IOC容器
            ///直接使用注册类
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<ServiceTest1>();
            ///serviceprovider实现了IDisposable接口
            using (ServiceProvider serviceProvider=services.BuildServiceProvider())
            {
                ///采用服务定位器的写法
                ServiceTest1 serviceTest1 = serviceProvider.GetService<ServiceTest1>();
                serviceTest1.Name = "Zhang";
                serviceTest1.SayHi();
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
