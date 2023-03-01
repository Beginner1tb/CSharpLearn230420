using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dll_Test1;
using Microsoft.Extensions.DependencyInjection;

namespace Dll_TestUse
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();
            Console.WriteLine(class1.TestDll1("12334") );

            ServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddScoped<IPrint, PrintOtherImp>();
            serviceDescriptors.AddScoped<Paper>();
            using (ServiceProvider serviceProvider=serviceDescriptors.BuildServiceProvider())
            {
                var paper = serviceProvider.GetRequiredService<Paper>();
                paper.PaperDisplay("DLL Test Use");
            }



            Console.ReadKey();
        }
    }
}
