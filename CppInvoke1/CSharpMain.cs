using System;
using System.Runtime.InteropServices;

namespace CppInvoke1
{
   // [StructLayout(LayoutKind.Sequential)]

    class CSharpMain
    {
        public struct MyStruct
        {
            public int a;
            public int b;
        }

        public const string dllPath = @"D:\CSharp\CppBuild\x64\Debug\CppDll1.dll";

        [DllImport(dllPath,CallingConvention=CallingConvention.Cdecl)]
        private static extern int Add(int a, int b);


        static void Main(string[] args)
        {

            int a = 1, b = 2;
            Console.WriteLine(Add(a, b));
        }
    }
}
