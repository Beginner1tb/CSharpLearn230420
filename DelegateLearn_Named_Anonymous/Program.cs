using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateLearn_Named_Anonymous
{
    delegate void Del(int i);
    class Program
    {
        static void Main(string[] args)
        {
            //程序入口所在的类也是可以实例化
            Program program = new Program();
            //因为AddTest方法是Program类的成员，所以要使用实例化的program对象
            Del del = program.AddTest;
            for (int i = 0; i < 2; i++)
            {
                del(i);
            }
            Console.WriteLine("////////////////////");
            //委托对于静态方法的使用与普通字段，方法的使用一样
            OuterClass outerClass = new OuterClass();
            Del del1 = outerClass.InstanceMethod;
            Del del2 = OuterClass.StaticMethod;
            del1(3);
            del2(4);
            Console.ReadKey();
        }

        /// <summary>
        /// AddTest方法与Main在一个类里的使用
        /// </summary>
        /// <param name="j"></param>
        void AddTest(int j)
        {
            Console.WriteLine($"Result: {j}+1={j++}");
        }
    }

    class OuterClass
    {
        public void InstanceMethod(int i)
        {
            Console.WriteLine("This is InstanceMethod" + i);
        }

        public static void StaticMethod(int i)
        {
            Console.WriteLine("This is StaticMethod" + i);
        }
    }
}
