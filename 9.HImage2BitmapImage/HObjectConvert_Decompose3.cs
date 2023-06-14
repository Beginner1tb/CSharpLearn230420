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
    public class HObjectConvert_Decompose3
    {
        /// <summary>
        /// 不可用，算法执行效率过低
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <returns></returns>
        [Obsolete("algorithm efficiency too low")]
        public static Bitmap HObjectToBitmap(HObject ho_Image)
        {
            Bitmap bmpImage = null;
            HObject ho_R, ho_G, ho_B;
            HOperatorSet.GenEmptyObj(out ho_R);
            HOperatorSet.GenEmptyObj(out ho_G);
            HOperatorSet.GenEmptyObj(out ho_B);
            HTuple hv_Width, hv_Height;
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.Decompose3(ho_Image, out ho_R, out ho_G, out ho_B);
            byte[] bR = new byte[hv_Width * hv_Height];
            byte[] bG = new byte[hv_Width * hv_Height];
            byte[] bB = new byte[hv_Width * hv_Height];
            ///正确写法
            for (int i = 0; i < hv_Width.I; i++)
            {
                for (int j = 0; j < hv_Height.I; j++)
                {
                    HOperatorSet.GetGrayval(ho_R, j, i, out HTuple kR);
                    bR[hv_Width * j + i] = (byte)kR.I;
                    HOperatorSet.GetGrayval(ho_G, j, i, out HTuple kG);
                    bG[hv_Width * j + i] = (byte)kG.I;
                    HOperatorSet.GetGrayval(ho_B, j, i, out HTuple kB);
                    bB[hv_Width * j + i] = (byte)kB.I;

                }
            }

            ///错误写法，像素值方向为先y再x
            //for (int i = 0; i < hv_Height.I; i++)
            //{
            //    for (int j = 0; j < hv_Width.I; j++)
            //    {
            //        HOperatorSet.GetGrayval(ho_R, i, j, out HTuple kR);
            //        bR[hv_Height * j + i] = (byte)kR.I;
            //        HOperatorSet.GetGrayval(ho_G, i, j, out HTuple kG);
            //        bG[hv_Height * j + i] = (byte)kG.I;
            //        HOperatorSet.GetGrayval(ho_B, i, j, out HTuple kB);
            //        bB[hv_Height * j + i] = (byte)kB.I;

            //    }
            //}

            ///
            //HOperatorSet.GetGrayval(ho_R, hv_Height, hv_Width, out bR);
            //HOperatorSet.GetGrayval(ho_G, out bG);
            //HOperatorSet.GetGrayval(ho_B, out bB);
            bmpImage = new Bitmap(hv_Width.I, hv_Height.I, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, hv_Width.I, hv_Height.I);
            BitmapData bmpData = bmpImage.LockBits(rect, ImageLockMode.ReadWrite, bmpImage.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmpImage.Height;
            byte[] rgbValues = new byte[bytes];
            for (int i = 0; i < hv_Height.I; i++)
            {
                for (int j = 0; j < hv_Width.I; j++)
                {
                    rgbValues[(i * bmpData.Stride) + (j * 3)] = bB[i * hv_Width.I + j];
                    rgbValues[(i * bmpData.Stride) + (j * 3) + 1] = bG[i * hv_Width.I + j];
                    rgbValues[(i * bmpData.Stride) + (j * 3) + 2] = bR[i * hv_Width.I + j];
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmpImage.UnlockBits(bmpData);
            ho_R.Dispose();
            ho_G.Dispose();
            ho_B.Dispose();
            return bmpImage;
        }

    }
}
