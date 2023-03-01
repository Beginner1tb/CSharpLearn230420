using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Dll_Test1;


namespace _2.DI_Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddScoped<ILog, LogImp1>();
            serviceDescriptors.AddScoped<IStorage, StorageImp1>();
            serviceDescriptors.AddScoped<IConfig, CfgImp1>();
            serviceDescriptors.AddScoped<Controller>();
            using (ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider())
            {
                var log = serviceProvider.GetRequiredService<ILog>();
                log.Log("logzzzz");

                var controller = serviceProvider.GetService<Controller>();
                controller.Test();
                
            }

            
            Console.ReadLine();
        }
    }

    class Controller
    {
        private readonly ILog log;
        private readonly IStorage storage;
        public Controller(ILog _log, IStorage _storage)
        {
            this.log = _log;
            this.storage = _storage;
        }

        public void Test()
        {
            log.Log("Begin Update");
            storage.Save("sssss");

        }
    }

    public interface ILog
    {
        void Log(string s);
    }

    class LogImp1 : ILog
    {
        public void Log(string s)
        {
            Console.WriteLine(s);
        }
    }

    interface IConfig
    {
        string GetValue(string name);
    }

    class CfgImp1 : IConfig
    {
        public string GetValue(string name)
        {
            return name;
        }
    }

    interface IStorage
    {
        void Save(string Content);
    }

    class StorageImp1 : IStorage
    {
        private readonly IConfig config;

        public StorageImp1(IConfig config)
        {
            this.config = config;
        }

        public void Save(string content)
        {
            string server = config.GetValue("server");
            Console.WriteLine(server);
        }
    }
}
