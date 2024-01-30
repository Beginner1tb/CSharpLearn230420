using System;


namespace _19.Injection_Method
{
    class Program
    {
        static void Main(string[] args)
        {
            //普通实例化方式
            Data1 data1 = new Data1();
            GenericData genericData = new GenericData(data1);
            genericData.WriteData();
            genericData.PubResult(1, 2);


            //构造函数注入方式,注意Data1是IData的实现，与基类和派生类的写法相同，都是实现了多态性
            IData data = new Data1();
            DataManager dataManager = new DataManager(data);
            dataManager.WriteData();
            dataManager.PubResult(2, 3);
        }
    }

    public class GenericData
    {
        //区别1. 需要在类内部实例化依赖项
        private readonly Data1 _data1;
        public GenericData(Data1 data1)
        {
            //实例化时自动执行构造函数
            Console.WriteLine("这是一个构造函数");
            //WriteData();
            _data1 = data1;
        }

        public void WriteData()
        {
           
            //Data1 data1 = new Data1();

            string strData = _data1.GetDate();
            Console.WriteLine("Generic Method Output:" + strData); ;
        }

        public void PubResult(int a,int b)
        {
            //区别2. 类依赖于具体的实现，多个实现可能就需要多个类实例化
           
            //Data1 data1 = new Data1();
            int res = _data1.ResNum(a, b);
            Console.WriteLine("Generic Method Output Number:" + res.ToString()) ;
        }
    }

    //简单用法
    public class GenericData1
    {
        
        public GenericData1()
        {
            //实例化时自动执行构造函数
            Console.WriteLine("这是一个构造函数");
            //WriteData();
            
        }

        public void WriteData()
        {

            Data1 data1 = new Data1();

            string strData = data1.GetDate();
            Console.WriteLine("Generic Method Output:" + strData); ;
        }

        public void PubResult(int a, int b)
        {
            

            Data1 data1 = new Data1();
            int res = data1.ResNum(a, b);
            Console.WriteLine("Generic Method Output Number:" + res.ToString());
        }
    }

    public class DataManager
    {
        private readonly IData _data;
        public DataManager(IData data)
        {
            _data = data;
        }
        public void WriteData()
        {
            string strData = _data.GetDate();
            Console.WriteLine("Interface Output:" + strData) ;
        }

        public void PubResult(int a, int b)
        {
            
            int res = _data.ResNum(a, b);
            Console.WriteLine("Interface Output Number:" + res.ToString());
        }
    }


}
