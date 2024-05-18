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
            Console.WriteLine("---------------------");
            Data2 data2 = new Data2();
            GenericData2 genericData2 = new GenericData2(data2);
            genericData2.WriteData();
            genericData2.PubResult(1, 2);

            Console.WriteLine("---------------------");

            //构造函数注入方式,注意Data1是IData的实现，与基类和派生类的写法相同，都是实现了多态性
            IData data = new Data1();
            DataManager dataManager = new DataManager(data);
            dataManager.WriteData();
            dataManager.PubResult(2, 3);
            Console.WriteLine("---------------------");
            //注意此处只修改了new Data2(),指定IData的实现类和传入的参数即可，不需要更改应用方法所在的类
            IData data22 = new Data2();
            DataManager dataManager2 = new DataManager(data22);
            dataManager2.WriteData();
            dataManager2.PubResult(2, 3);

            Console.WriteLine("---------------------");
            string s = nameof(Data1);
            Console.WriteLine(nameof(Data1));

        }
    }

    public class GenericData
    {
        //区别1. 需要在类内部实例化依赖项，注意，这边只对Data1类有用
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

        public void PubResult(int a, int b)
        {


            //Data1 data1 = new Data1();
            int res = _data1.ResNum(a, b);
            Console.WriteLine("Generic Method Output Number:" + res.ToString());
        }
    }

    //区别2. 类依赖于具体的实现，多个接口实现可能就需要多个类实例化
    //此处仅为示例所以类采用不同名称
    public class GenericData2
    {
        //区别1. 需要在类内部实例化依赖项，注意，这边只对Data2类有用
        private readonly Data2 _data2;
        public GenericData2(Data2 data2)
        {
            //实例化时自动执行构造函数
            Console.WriteLine("这是一个构造函数");
            //WriteData();
            _data2 = data2;
        }

        public void WriteData()
        {

            //Data1 data1 = new Data1();

            string strData = _data2.GetDate();
            Console.WriteLine("Generic Method Output:" + strData); ;
        }

        public void PubResult(int a, int b)
        {
            //Data1 data1 = new Data1();
            int res = _data2.ResNum(a, b);
            Console.WriteLine("Generic Method Output Number:" + res.ToString());
        }
    }

    //简单用法，应用方法所属类的每个方法都单独实例化
    //此处不使用构造函数初始化
    public class GenericData1
    {


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
            Console.WriteLine("Interface Output:" + strData);
        }

        public void PubResult(int a, int b)
        {

            int res = _data.ResNum(a, b);
            Console.WriteLine("Interface Output Number:" + res.ToString());
        }
    }


}
