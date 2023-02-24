using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Using_HowToUse
{
    class Program
    {
        static void Main(string[] args)
        {
            //错误，必须是现实了IDisposable接口的方法
            //using (int i = 10) { }
            string str = "123";
            using (StringReader reader=new StringReader(str))
            {
                Console.WriteLine(reader.ReadLine());

                //相当于
                //try
                //{
                //    Console.WriteLine(reader.ReadLine());
                //}
                //finally
                //{
                //    reader?.Dispose();
                //}
            }
            //C#8.0新写法,不用括号
            //using var x = new StringReader(str);
            //Console.WriteLine(reader.ReadLine());

            Console.ReadKey();
        }
    }
}
