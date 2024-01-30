using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19.Injection_Method
{
    public interface IData
    {
         string GetDate();
        int ResNum(int a, int b);
    }

    public class Data1 : IData
    {
        public string GetDate()
        {
            return "GetData Method1";
        }

        public int ResNum(int a, int b)
        {
            return a + b;
        }
    }

    public class Data2 : IData
    {
        public string GetDate()
        {
            return "GetData Method2";
        }

        public int ResNum(int a, int b)
        {
            return a + b+10000;
        }
    }
}
