using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exeception_Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream file = null;
            FileInfo fileInfo = new FileInfo("./test3.txt");
            try
            {              
                file = fileInfo.Open(FileMode.OpenOrCreate);
                ///注意OpenWrite()方法，就算先创建文件再写入,相当于OpenOrCreate
                //file = fileInfo.OpenWrite();
                byte[] testByte = Encoding.ASCII.GetBytes("12345");
                file.Write(testByte, 0, 3);
            }
            //指定Exception类型捕捉
            catch (Exception e)
            {
                Console.WriteLine("exception Type is :" + e.GetType());
                Console.WriteLine("Exception Info is:" + e.Message);
                throw;
            }
            ///最后结束try的过程
            finally
            {               
                file?.Close();
            }

            //可以catch多次
            int num = 100;
            int div = 10;
            DateTime dateTime = DateTime.Now;
            try
            {
                do
                {
                    int res = num / div;
                    div--;
                    //if (div==0)
                    //{
                    //    break;
                    //}
                } while (true);
            }
            catch (Exception) when (dateTime.ToString("HH-mm") == "14-09")
            {

                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
