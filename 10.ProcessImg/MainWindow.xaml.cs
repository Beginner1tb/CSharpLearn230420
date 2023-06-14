using System.Windows;
using System.Windows.Controls;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;
using System.Collections.Generic;

namespace _10.ProcessImg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap bitmap_orin = null;
        public Bitmap bitmap_init = null;
        public List<BitmapImage> bitmaps_show = new List<BitmapImage>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CBox_ProcessImg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bitmap_orin != null)
            {
                switch (CBox_ProcessImg.SelectedIndex)
                {
                    case 0:
                        img_show1.Source = null;
                        img_show1.Source = bitmaps_show[0];

                        break;
                    case 1:
                        img_show1.Source = null;
                        img_show1.Source = bitmaps_show[1];
                        break;
                    case 2:
                        img_show1.Source = null;
                        img_show1.Source = bitmaps_show[2];
                        break;
                    case 3:
                        img_show1.Source = null;
                        img_show1.Source = bitmaps_show[3];
                        break;
                    default:
                        break;
                }
            }
        }

        private void Btn_LoadFile_Click(object sender, RoutedEventArgs e)
        {

            bitmaps_show.Clear();

            string filepath;
            ///注意.Net 5下操作的不同
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = "C:\\Users\\LFLFLF\\Desktop\\",
                Filter = "图像文件|*.bmp",
                RestoreDirectory = true
            };


            if (openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;



                /////从文件读取到内存流中
                //Stream imageStream = new MemoryStream();
                //byte[] imageData = File.ReadAllBytes(filepath);
                //imageStream.Write(imageData, 0, imageData.Length);

                /////注意.Net 5引用的是System.Drawing.Common的nuget包老版本
                //Bitmap bitmap = new Bitmap(imageStream);

                bitmap_init = new Bitmap(filepath);
                ///调整显示图像的大小
                //Bitmap bitmapResize = new Bitmap(bitmap_init, 600, 600);
                bitmap_orin = BitmapResize(bitmap_init);
                bitmaps_show.Add(ImageControlShow(BitmapResize(bitmap_init)));

                //img_show1.Source = ImageControlShow(bitmapResize);


                //img_show1.Source = null;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap_init.Width, bitmap_init.Height);

                if (IsColorBitmap(bitmap_init))
                {
                    BitmapData bitmapData = bitmap_init.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    {
                        HObject ho_Color, ho_red, ho_green, ho_blue;
                        HObject ho_h, ho_s, ho_v, ho_Region, ho_Image, ho_ImageResult;

                        HTuple hv_Width = new HTuple(), hv_Height = new HTuple();

                        HOperatorSet.GenEmptyObj(out ho_Color);
                        HOperatorSet.GenEmptyObj(out ho_red);
                        HOperatorSet.GenEmptyObj(out ho_green);
                        HOperatorSet.GenEmptyObj(out ho_blue);
                        HOperatorSet.GenEmptyObj(out ho_h);
                        HOperatorSet.GenEmptyObj(out ho_s);
                        HOperatorSet.GenEmptyObj(out ho_v);
                        HOperatorSet.GenEmptyObj(out ho_Region);
                        HOperatorSet.GenEmptyObj(out ho_Image);
                        HOperatorSet.GenEmptyObj(out ho_ImageResult);
                        ho_Color.Dispose();


                        // HOperatorSet.GenImage1(out ho_Monkey, "byte", bitmap.Width, bitmap.Height, bitmapData.Scan0);
                        HOperatorSet.GenImageInterleaved(out ho_Color, bitmapData.Scan0, "bgr", bitmap_init.Width, bitmap_init.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                        //HOperatorSet.ReadImage(out ho_Color, "C:/Users/LFLFLF/Desktop/color.bmp");
                        ho_red.Dispose(); ho_green.Dispose(); ho_blue.Dispose();
                        HOperatorSet.Decompose3(ho_Color, out ho_red, out ho_green, out ho_blue);
                        hv_Width.Dispose(); hv_Height.Dispose();
                        HOperatorSet.GetImageSize(ho_Color, out hv_Width, out hv_Height);
                        ho_h.Dispose(); ho_s.Dispose(); ho_v.Dispose();
                        HOperatorSet.TransFromRgb(ho_red, ho_green, ho_blue, out ho_h, out ho_s, out ho_v,
                            "hsv");
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_h, out ho_Region, 128, 255);
                        ho_Image.Dispose();
                        HOperatorSet.GenImageConst(out ho_Image, "byte", hv_Width, hv_Height);
                        ho_ImageResult.Dispose();
                        HOperatorSet.PaintRegion(ho_Region, ho_Image, out ho_ImageResult, 255, "fill");


                        HObject2Bitmap(ho_red, out Bitmap bmp_Red);
                        bitmaps_show.Add(ImageControlShow(BitmapResize(bmp_Red)));

                        HObject2Bitmap(ho_s, out Bitmap bmp_S);
                        bitmaps_show.Add(ImageControlShow(BitmapResize(bmp_S)));


                        HObject2Bitmap(ho_ImageResult, out Bitmap bmp_Thres);
                        bitmaps_show.Add(ImageControlShow(BitmapResize(bmp_Thres)));

                        ho_Color.Dispose();
                        ho_red.Dispose();
                        ho_green.Dispose();
                        ho_blue.Dispose();
                        ho_h.Dispose();
                        ho_s.Dispose();
                        ho_v.Dispose();
                        ho_Region.Dispose();
                        ho_Image.Dispose();
                        ho_ImageResult.Dispose();
                        bmp_Red.Dispose();
                        bmp_S.Dispose();
                        bmp_Thres.Dispose();
                        hv_Height.Dispose();
                        hv_Width.Dispose();



                    }
                    bitmap_init.UnlockBits(bitmapData);

                }
                else
                {
                    BitmapData bitmapData = bitmap_init.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                    {




                    }
                    bitmap_init.UnlockBits(bitmapData);
                }
                img_show1.Source = bitmaps_show[CBox_ProcessImg.SelectedIndex];
                // imageStream.Flush();
                //bitmapResize.Dispose();
                bitmap_init.Dispose();

                //imageStream.Close();
                // imageData = null;
                //GC.Collect();

            }





        }

        public BitmapImage ImageControlShow(Bitmap bitmap)
        {
            BitmapImage bitmapImage;
            using (MemoryStream memoryStream = new MemoryStream())
            {

                bitmap.Save(memoryStream, ImageFormat.Jpeg);
                memoryStream.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            GC.Collect();
            return bitmapImage;


        }


        bool IsColorBitmap(Bitmap bitmap)
        {
            PixelFormat format = bitmap.PixelFormat;
            return format == PixelFormat.Format32bppArgb || format == PixelFormat.Format32bppRgb
                || format == PixelFormat.Format24bppRgb || format == PixelFormat.Format32bppPArgb;
        }

        public Bitmap HObject2Bitmap24(HObject hObject)
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

        public void HObject2Bitmap(HObject image, out Bitmap bitmap)
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

        public Bitmap BitmapResize(Bitmap bitmap)
        {
            Bitmap bitmapResize = new Bitmap(bitmap, 600, 600);
            return bitmapResize;
        }

    }
}
