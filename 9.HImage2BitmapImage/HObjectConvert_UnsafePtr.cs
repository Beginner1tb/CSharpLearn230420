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
using HalconDotNet;
using System.Runtime.InteropServices;

namespace _9.HImage2BitmapImage
{
    public class HObjectConvert_UnsafePtr
    {
        [Obsolete("Efficiency too low")]
        public static Bitmap HObject2Bitmap24Ptr(HObject hObject)
        {
            try
            {
                HTuple r, g, b, type, width, height;
                unsafe
                {
                    HTuple rr, gg, bb, type1, width1, height1;
                    HOperatorSet.GetImagePointer3(hObject, out rr, out gg, out bb, out type1, out width1, out height1);
                    IntPtr k = ((IntPtr)rr.I);
                    byte* kkk = (byte*)rr.L;
                    int sss = kkk[0];
                    byte[] kk = new byte[width1 * height1];
                    Marshal.Copy(rr, kk, 0, width1 * height1);
          
                }
                HOperatorSet.GetImagePointer3(hObject, out r, out g, out b, out type, out width, out height);

                //用指针效果相近
                IntPtr r1 = (IntPtr)r;
                IntPtr g1 = (IntPtr)g;
                IntPtr b1 = (IntPtr)b;

                byte[] red = new byte[width * height];
                byte[] green = new byte[width * height];
                byte[] blue = new byte[width * height];
                // 将指针指向地址的值取出来放到byte数组中
                Marshal.Copy(r1, red, 0, width * height);
                Marshal.Copy(g1, green, 0, width * height);
                Marshal.Copy(b1, blue, 0, width * height);

                Bitmap bitmap = new Bitmap(width.I, height.I, PixelFormat.Format24bppRgb);
                Rectangle rect = new Rectangle(0, 0, width.I, height.I);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                unsafe
                {                  
                    byte* bptr = (byte*)bitmapData.Scan0;
                    //必须用.L，不然32位长度不对，会报指针错误
                    byte* hr = (byte*)r.L;
                    byte* hg = (byte*)g.L;
                    byte* hb = (byte*)b.L;

                    for (int i = 0; i < width * height; i++)
                    {
                        bptr[i * 3] = hb[i];
                        bptr[i * 3 + 1] = hg[i];
                        bptr[i * 3 + 2] = hr[i];
                        //两个性能差不多
                        //bptr[i * 3] = blue[i];
                        //bptr[i * 3 + 1] = green[i];
                        //bptr[i * 3 + 2] = red[i];
                    }
                    
                }
                bitmap.UnlockBits(bitmapData);
                return bitmap;
            }
            catch (Exception ex)
            {

                throw new Exception("HObject转24BMP错误", ex);
            }

        }

        public static Bitmap HObject2Bitmap32Ptr(HObject hObject)
        {
            try
            {
                HTuple r, g, b, type, width, height;
                HOperatorSet.GetImagePointer3(hObject, out r, out g, out b, out type, out width, out height);
                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                
                unsafe
                {
                    byte* bptr = (byte*)bitmapData.Scan0;
                    byte* hr = ((byte*)r.I);
                    byte* hg = ((byte*)g.I);
                    byte* hb = ((byte*)b.I);
                    for (int i = 0; i < width * height; i++)
                    {
                        bptr[i * 4] = (hb)[i];
                        bptr[i * 4 + 1] = (hg)[i];
                        bptr[i * 4 + 2] = (hr)[i];
                        bptr[i * 4 + 3] = 255;
                    }
                    
                }
                bitmap.UnlockBits(bitmapData);
                return bitmap;
            }
            catch (Exception ex)
            {

                throw new Exception("HObject转32BMP错误", ex);
            }

        }
    }
}
