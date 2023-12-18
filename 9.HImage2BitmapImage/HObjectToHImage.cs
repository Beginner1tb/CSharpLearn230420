using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace _9.HImage2BitmapImage
{
    public class HObjectToHImage
    {
        /// <summary>
        /// HObject转HImage黑白
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static HImage HObject2Bitmap(HObject hObject)
        {
            HImage hImage = new HImage();
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hObject, out pointer, out type, out width, out height);
            hImage.GenImage1(type, width, height, pointer);
            return hImage;
        }

        /// <summary>
        /// HObject转HImage彩色
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static HImage HObject2BitmapRGB(HObject hObject)
        {
            HImage hImage = new HImage();
            HTuple PtrRed, PtrGreen, PtrBlue, type, width, height;
            HOperatorSet.GetImagePointer3(hObject, out PtrRed, out PtrGreen, out PtrBlue, out type, out width, out height);
            hImage.GenImage3(type, width, height, PtrRed, PtrGreen, PtrBlue);
            return hImage;

        }
    }
}
