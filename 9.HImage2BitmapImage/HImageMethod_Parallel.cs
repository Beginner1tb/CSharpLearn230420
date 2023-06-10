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
    public class HImageMethod_Parallel
    {
        /// <summary>
        /// 可用，HImage转Bitmap彩色和黑白二合一
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <returns></returns>
        public static Bitmap HImageToBitmap(HImage ho_Image)
        {
            int iWidth, iHeight, iNumChannels;
            IntPtr ip_R, ip_G, ip_B, ip_Gray;
            String sType;
            // null return object
            Bitmap bitmap = null;
            try
            {
                //
                // Note that pixel data is stored differently in System.Drawing.Bitmap
                // a) Stride:
                // stride is the width, rounded up to a multiple of 4 (padding)
                // Size of data array HALCON: heigth*width, Bitmap: heigth*stride
                // compare: https://msdn.microsoft.com/en-us/library/zy1a2d14%28v=vs.110%29.aspx
                // b) RGB data
                // HALCON: three arrays, Bitmap: one array (alternating red/green/blue)
                iNumChannels = ho_Image.CountChannels();
                if (iNumChannels != 1 && iNumChannels != 3)
                    throw new Exception("Conversion of HImage to Bitmap failed. Number of channels of the HImage is: " +
                        iNumChannels + ". Conversion rule exists only for images with 1 or 3 chanels");
                if (iNumChannels == 1)
                {
                    //
                    // 1) Get the image pointer
                    ip_Gray = ho_Image.GetImagePointer1(out sType, out iWidth, out iHeight);
                    //
                    // 2) Calculate the stride
                    int iPadding = (4 - (iWidth % 4)) % 4;
                    int iStride = iWidth + iPadding;
                    //
                    // 3) Create a new gray Bitmap object, allocating the necessary (managed) memory 
                    bitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format8bppIndexed);
                    // note for high performance: in case of padding=0, image can be copied by reference.
                    // however, then the bitmap's validity relies on the HImage lifetime.
                    // bitmap = new Bitmap(iWidth, iHeight, iWidth, PixelFormat.Format8bppIndexed, ip_Gray);
                    //
                    // 4) Copy the image data directly into the bitmap data object, re-arranged in the required bitmap order
                    // BitmapData lets us access the data in memory
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, iWidth, iHeight),
                        ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    // System.Threading.Tasks.Parallel processing requires .NET framework >= 4.0 
                    Parallel.For(0, iHeight, r =>
                    {
                        IntPtr posRead = ip_Gray + r * iWidth;
                        IntPtr posWrite = bmpData.Scan0 + r * iStride;
                        for (int c = 0; c < iWidth; c++)
                            Marshal.WriteByte((IntPtr)posWrite, c, Marshal.ReadByte((IntPtr)posRead, c));
                    });
                    //
                    // 5) Let the windows memory management take over control
                    bitmap.UnlockBits(bmpData);
                    //
                    // 6) Adjust palette to grayscale (linearized grayscale)
                    // ColorPalette has no constructor -> obtain it from the static member
                    ColorPalette cp_P = bitmap.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        cp_P.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                    }
                    bitmap.Palette = cp_P;
                }
                if (iNumChannels == 3)
                {
                    //
                    // 1) Get the image pointer
                    ho_Image.GetImagePointer3(out ip_R, out ip_G, out ip_B, out sType, out iWidth, out iHeight);
                    //
                    // 2) Calculate the stride
                    int iPadding = (4 - ((iWidth * 3) % 4)) % 4;
                    int iStride = iWidth * 3 + iPadding;
                    //
                    // 3) Create a new RGB Bitmap object, allocating the necessary (managed) memory 
                    bitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                    //
                    // 4) Copy the image data directly into the bitmap data object, re-arranged in the required bitmap order
                    // BitmapData lets us access the data in memory
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, iWidth, iHeight),
                    ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    Parallel.For(0, iHeight, r =>
                    {
                        IntPtr posReadR = (IntPtr)((long)ip_R + r * iWidth);
                        IntPtr posReadG = (IntPtr)((long)ip_G + r * iWidth);
                        IntPtr posReadB = (IntPtr)((long)ip_B + r * iWidth);
                        IntPtr posWrite = (IntPtr)((long)bmpData.Scan0 + r * iStride);
                        for (int c = 0; c < iWidth; c++)
                        {
                            Marshal.WriteByte(posWrite, 3 * c, Marshal.ReadByte(posReadB, c));
                            Marshal.WriteByte(posWrite, 3 * c + 1, Marshal.ReadByte(posReadG, c));
                            Marshal.WriteByte(posWrite, 3 * c + 2, Marshal.ReadByte(posReadR, c));
                        }
                    });
                    //
                    // 5) Let the windows memory management take over control
                    bitmap.UnlockBits(bmpData);


                }

                GC.Collect();
            }
            catch (Exception ex)
            {
                throw new Exception("Conversion of HImage to Bitmap failed.", ex);
            }
            return bitmap;
        }
    }
}
