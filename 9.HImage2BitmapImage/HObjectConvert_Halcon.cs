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

namespace _9.HImage2BitmapImage
{
    public class HObjectConvert_Halcon
    {
        /// <summary>
        /// 可用，HObject转Bitmap彩色
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static Bitmap HObject2Bitmap24(HObject hObject)
        {
            try
            {
                HTuple hpoint, type, width, height, width0, height0;
                HObject interImage = new HObject();
                HOperatorSet.GetImageSize(hObject, out width0, out height0);
                HOperatorSet.InterleaveChannels(hObject, out interImage, "rgb", 4 * width0, 0);
                HOperatorSet.GetImagePointer1(interImage, out hpoint, out type, out width, out height);
                IntPtr ptr = hpoint;
                Bitmap bitmap = new Bitmap(width / 4, height, width, PixelFormat.Format24bppRgb, ptr);
                GC.Collect();
                return bitmap;
            }
            catch (Exception ex)
            {

                throw new Exception("HObject转24BMP错误", ex);
            }


        }
    }
}
