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
    public class HObjectConvert_IntPtr
    {
        /// <summary>
        /// 可用，HObject转黑白bitmap
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bitmap"></param>
        public static void HObject2Bitmap(HObject image, out Bitmap bitmap)
        {
            HTuple hpoint, type, width, height;
            const int Alpha = 255;
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);
            bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = bitmap.Palette;
            for (int i = 0; i < 255; i++)
            {
                pal.Entries[i] = System.Drawing.Color.FromArgb(Alpha, i, i, i);
            }
            bitmap.Palette = pal;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int pixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            IntPtr intPtr1 = bitmapData.Scan0;
            IntPtr intPtr2 = hpoint;
            int bytes = width * height;
            byte[] rgbvalue = new byte[bytes];
            Marshal.Copy(intPtr2, rgbvalue, 0, bytes);
            Marshal.Copy(rgbvalue, 0, intPtr1, bytes);
            bitmap.UnlockBits(bitmapData);
        }
    }
}
