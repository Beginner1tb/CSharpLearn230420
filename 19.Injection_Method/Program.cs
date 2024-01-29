using System;


namespace _19.Injection_Method
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericData genericData = new GenericData();
            genericData.WriteData();
        }
    }

    public class GenericData
    {
       
        public GenericData()
        {
        }

        public void WriteData()
        {
            Data1 data1 = new Data1();
            Console.WriteLine("Output:" + data1.GetDate()); ;
        }
    }
}
