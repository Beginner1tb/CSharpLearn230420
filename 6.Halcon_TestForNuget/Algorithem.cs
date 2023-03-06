using HalconDotNet;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _6.Halcon_TestForNuget
{
    public class Algorithem
    {
        public Out1 Algorithm_Test1(byte[] imageData)
        {
            Stream imageStream = new MemoryStream();
            imageStream.Write(imageData, 0, imageData.Length);
            Bitmap bitmap = new Bitmap(imageStream);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            {


                HImage hImage = new HImage();



                HObject ho_Monkey, ho_Region;


                HTuple hv_Height = new HTuple(), hv_Width = new HTuple();
                HTuple hv_Ratio = new HTuple(), hv_img_h = new HTuple();

                HOperatorSet.GenEmptyObj(out ho_Monkey);
                HOperatorSet.GenEmptyObj(out ho_Region);
                ho_Monkey.Dispose();

                ///通过文件载入
                //HOperatorSet.ReadImage(out ho_Monkey,filepath);
                ///通过数据流写入
                HOperatorSet.GenImage1(out ho_Monkey, "byte", bitmap.Width, bitmap.Height, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);


                ho_Region.Dispose();

                HOperatorSet.Threshold(ho_Monkey, out ho_Region, 128, 255);
                hv_Height.Dispose(); hv_Width.Dispose(); hv_Ratio.Dispose();
                HOperatorSet.HeightWidthRatio(ho_Region, out hv_Height, out hv_Width, out hv_Ratio);
                hv_img_h.Dispose();
                hv_img_h = new HTuple(hv_Height);
                Out1 out1 = new Out1();
                out1.height1 = hv_Height.I;
                out1.width1 = hv_Width.I;
                

                //imageTest_Height = hv_img_h.I;
                ho_Monkey.Dispose();
                ho_Region.Dispose();

                hv_Height.Dispose();
                hv_Width.Dispose();
                hv_Ratio.Dispose();
                hv_img_h.Dispose();

                return out1;
            }
        }

    }

    public class Out1
    {
        public int height1 { get; set; }
        public int width1 { get; set; }
    }
}
