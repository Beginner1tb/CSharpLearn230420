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
    }

    public class Data1 : IData
    {
        public string GetDate()
        {
            return "GetData Method";
        }
    }
}
