using System;
using System.Reflection;

namespace Reflection_Learn1
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Type type = typeof(Person);
            object person = Activator.CreateInstance(type);
            PropertyInfo prop = type.GetProperty("Name");
            PropertyInfo[] propertyInfos = type.GetProperties();
            prop.SetValue(person, "John Doe");
            Console.WriteLine(prop.GetValue(person));
            Console.WriteLine("------------------------");


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

}
