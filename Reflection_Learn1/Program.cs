using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Reflection_Learn1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("简单调用");
            Type type = typeof(Person);
            //手动实例化
            object person = Activator.CreateInstance(type);
            PropertyInfo prop = type.GetProperty("Name");
            PropertyInfo[] propertyInfos = type.GetProperties();
            prop.SetValue(person, "John Doe");
            Console.WriteLine(prop.GetValue(person));
            Console.WriteLine("------------------------");
            Console.WriteLine("通过反射获取方法，并用委托做包装");
            //用原类做实例化
            Calculator calculator = new Calculator();
            MethodInfo methodInfo = calculator.GetType().GetMethod("Add");
            Func<int, int, int> addDelegate = (Func<int, int, int>)Delegate.CreateDelegate(typeof(Func<int, int, int>), calculator, methodInfo);
            var res = addDelegate(1, 3);
            Console.WriteLine(res);
            Console.WriteLine("-------------------------");
            Console.WriteLine("InvokeMember 和成员的重载方法");
            Type type_Invoke_Class = typeof(Invoke_Class);
            object invoke_example = Activator.CreateInstance(type_Invoke_Class);
            //如果构造函数带参数，new object[]{}中不带参数则会报错
            type_Invoke_Class.InvokeMember("Print_Static", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { "11111" });
            type_Invoke_Class.InvokeMember("Print", BindingFlags.InvokeMethod, null, invoke_example, new object[] { "Parament of print" });
            type_Invoke_Class.InvokeMember("Print", BindingFlags.InvokeMethod, null, invoke_example, new object[] { "Para1", "Para2" });
            type_Invoke_Class.InvokeMember("Print_None", BindingFlags.InvokeMethod, null, invoke_example, new object[] { });

            Console.WriteLine("-------------------------");
            Console.WriteLine("使用methodInfo方式调用方法");
            //使用methodInfo方式调用方法
            MethodInfo methodInfo_Invoke_Class1 = type_Invoke_Class.GetMethod("Print_None");
            //对象是实例化的invoke_example
            methodInfo_Invoke_Class1.Invoke(invoke_example, new object[] { });
            type_Invoke_Class.GetMethod("Print_Static").Invoke(null, new object[] { "GetMethod方法，静态" });
            type_Invoke_Class.GetMethod("Print", new Type[] { typeof(string) }).Invoke(invoke_example, new object[] { "GetMethod方法，非静态" });
            object[] str_ObjectTest1 = new object[] { "GetMethod方法,参数1", "GetMethod方法,参数2" };
            //重载需要通过type获取方法参数类型来判断是哪个方法
            type_Invoke_Class.GetMethod("Print", new Type[] { typeof(string), typeof(string) }).Invoke(invoke_example, str_ObjectTest1);

            Console.WriteLine("-------------------------");
            Console.WriteLine("反射与依赖注入");
            ServiceCollection sc = new ServiceCollection();
            sc.AddScoped(type_Invoke_Class);
            ServiceProvider sp = sc.BuildServiceProvider();
            var ss1 = sp.GetService<Invoke_Class>();
            ss1.Print("DI test");
    


        }


    }

    class Person
    {
        public string Name { get; set; }
    }

    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    public class Invoke_Class
    {
        public static void Print_Static(string str)
        {
            Console.WriteLine(str);
        }

        public void Print(string str)
        {
            Console.WriteLine(str);
        }

        public void Print(string str, string str2)
        {
            Console.WriteLine(str + str2);
        }

        public void Print_None()
        {
            Console.WriteLine("无参数print");
        }
    }
}
