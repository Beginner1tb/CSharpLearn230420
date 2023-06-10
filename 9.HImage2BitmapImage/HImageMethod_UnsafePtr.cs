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
    public class HImageMethod_unsafePtr
    {
        public static Bitmap HImage2Bitmap24Ptr(HImage hImage)
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
                Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                //Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format32bppRgb);
                Rectangle rect2 = new Rectangle(0, 0, w, h);
                BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                //BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                IntPtr intPtr = bitmapData2.Scan0;


                //
                unsafe
                {

                    byte* bptr2 = (byte*)bitmapData2.Scan0;


                    for (int i = 0; i < w * h; i++)
                    {
                        bptr2[i * 3] = blue[i];
                        bptr2[i * 3 + 1] = green[i];
                        bptr2[i * 3 + 2] = red[i];

                    }

                }
                bitmap2.UnlockBits(bitmapData2);
                return bitmap2;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转24BMP错误", ex);
            }
        }


        public static Bitmap HImage2Bitmap32Ptr(HImage hImage)
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

                Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format32bppRgb);
                Rectangle rect2 = new Rectangle(0, 0, w, h);
                //BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                IntPtr intPtr = bitmapData2.Scan0;


                //
                unsafe
                {

                    byte* bptr2 = (byte*)bitmapData2.Scan0;
                    for (int i = 0; i < w * h; i++)
                    {
                        bptr2[i * 4] = blue[i];
                        bptr2[i * 4 + 1] = green[i];
                        bptr2[i * 4 + 2] = red[i];
                        bptr2[i * 4 + 3] = 255;
                    }


                }


                bitmap2.UnlockBits(bitmapData2);
                return bitmap2;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转32BMP错误", ex);
            }
        }

        public static Bitmap HImage2Bitmap8Ptr(HImage hImage)
        {
            try
            {
                IntPtr intPtr = hImage.GetImagePointer1(out string type, out int width, out int height);
                byte[] buffer = new byte[width * height];
                Marshal.Copy(intPtr, buffer, 0, buffer.Length);
                Bitmap bitmap3 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                Rectangle rect3 = new Rectangle(0, 0, width, height);
                // BitmapData bitmapData3 = bitmap3.LockBits(rect3, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                unsafe
                {
                    fixed (byte* bytepointer = buffer)
                    {

                        bitmap3 = new Bitmap(width, height, width, PixelFormat.Format8bppIndexed, new IntPtr(bytepointer));
                        ColorPalette colorPalette = bitmap3.Palette;
                        for (int i = 0; i < 255; i++)
                        {
                            colorPalette.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
                        }
                        bitmap3.Palette = colorPalette;
                    }
                }
                return bitmap3;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转8BMP错误", ex);
            }
        }


    }
}
