using System;
using System.Runtime.InteropServices;

namespace CppInvoke1
{
   // [StructLayout(LayoutKind.Sequential)]

    unsafe class CSharpMain
    {
        public class CoaxialAlgorithmObject
        {
            public virtual int Execute(char* ucpImageData, int iWidth, int iHeight, int iChannel)
            {
                return 0;
            }


            public virtual bool SaveFile(char* szFileName)
            {
                return false;
            }

            public virtual bool LoadFile(char* szFileName)
            {
                return false;
            }



        }

        public struct MyStruct
        {
            public int a;
            public int b;
        }

        public int* p;


        public const string dllPath = @"D:\CSharp\CppBuild\x64\Debug\CppDll1.dll";

        public const string dll1 = @"D:\TestFolder\算法\CoaxialAlgorithmDll.dll";

        //[DllImport(dll1, CallingConvention = CallingConvention.Cdecl)]
        //private static extern int fnShowParaWnd(int a, int b);



        [DllImport(dllPath,CallingConvention=CallingConvention.Cdecl)]
        private static extern int Add(int a, int b);
        [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern int foo1(int* p);
        //[DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        //private static extern int foo1(int* p);

        static void Main(string[] args)
        {

            int a = 1, b = 2;
            int[] arr = { 3, 4 };

            fixed (int* p = arr)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine("Value: " + *(p + i));
                }
            }


            CoaxialAlgorithmObject coaxial;
            //CoaxialAlgorithmObject* co = &coaxial;
            Console.WriteLine(Add(a, b));
            
        }
    }
}
