using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dll_Test1
{
    public class Class1
    {
        public string TestDll1(string s)
        {
            return $"This is a Test DLL{s}";
        }
    }

    public interface IPrint
    {
        void Output(string str);
    }

    public class PrintImp : IPrint
    {
        public void Output(string str)
        {
            Console.WriteLine("PrintImp :"+str);
        }
    }

    public class PrintOtherImp : IPrint
    {
        public void Output(string str)
        {
            Console.WriteLine("PrintOtherImp :"+str);
        }
    }

    public class Paper
    {
        private readonly IPrint print;
        public Paper(IPrint _print)
        {
            print = _print;
        }

        public void PaperDisplay(string str)
        {
            print.Output(str);
        }
    }
}
