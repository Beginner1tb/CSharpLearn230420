using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace _9.HImage2BitmapImage
{
    public class HImageMethod_Marshal
    {
        public static Bitmap HImage2Bitmap24Intptr(HImage hImage)
        {
            try
            {
                hImage.GetImagePointer3(out IntPtr r, out IntPtr g, out IntPtr b, out string type, out int w, out int h);
                byte[] red = new byte[w * h];
                byte[] green = new byte[w * h];
                byte[] blue = new byte[w * h];
                // 将指针指向地址的值取出来放到byte数组中
                Marshal.Copy(r, red, 0, w * h);
                Marshal.Copy(g, green, 0, w * h);
                Marshal.Copy(b, blue, 0, w * h);

                Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                Rectangle rect = new Rectangle(0, 0, w, h);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                IntPtr bptr = bitmapData.Scan0;
                for (int i = 0; i < red.Length; i++)
                {
                    Marshal.Copy(blue, i, bptr + i * 3, 1);
                    Marshal.Copy(green, i, bptr + i * 3 + 1, 1);
                    Marshal.Copy(red, i, bptr + i * 3 + 2, 1);

                }

                bitmap.UnlockBits(bitmapData);
                return bitmap;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转24BMP【IntPtr方法】错误", ex);
            }
        }

        public static Bitmap HImage2Bitmap32Intptr(HImage hImage)
        {
            try
            {
                hImage.GetImagePointer3(out IntPtr r, out IntPtr g, out IntPtr b, out string type, out int w, out int h);
                byte[] red = new byte[w * h];
                byte[] green = new byte[w * h];
                byte[] blue = new byte[w * h];
                // 将指针指向地址的值取出来放到byte数组中
                Marshal.Copy(r, red, 0, w * h);
                Marshal.Copy(g, green, 0, w * h);
                Marshal.Copy(b, blue, 0, w * h);
                Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format32bppRgb);
                // Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                Rectangle rect = new Rectangle(0, 0, w, h);
                // BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                IntPtr bptr = bitmapData.Scan0;
                for (int i = 0; i < red.Length; i++)
                {
                    Marshal.Copy(blue, i, bptr + i * 4, 1);
                    Marshal.Copy(green, i, bptr + i * 4 + 1, 1);
                    Marshal.Copy(red, i, bptr + i * 4 + 2, 1);
                    Marshal.Copy(new byte[] { 255 }, 0, bptr + i * 4 + 3, 1);
                }

                bitmap.UnlockBits(bitmapData);
                return bitmap;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转32BMP【IntPtr方法】错误", ex);
            }
        }
    }
}
