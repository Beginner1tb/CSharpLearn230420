using System;


namespace _15.UnitTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            addTest1();
        }

        public static int addTest1()
        {
            int num1, num2;
            num1 = 1;
            num2 = 2;
            Calc calc = new Calc();
            int num3 = calc.Addnumber(num1, num2);
            return num3;
        }
    }

    public class Calc
    {
        public int Addnumber(int a, int b)
        {
            return a + b;
        }
    }

    public class StrOut
    {
        public string SimpleStrOutput()
        {
            return "1111";
        }
    }
}
