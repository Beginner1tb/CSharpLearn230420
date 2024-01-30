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
            return "GetData Method";
        }

        public int ResNum(int a, int b)
        {
            return a + b;
        }
    }
}
