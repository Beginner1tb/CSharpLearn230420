using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HalconDotNet;
using Microsoft.Win32;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace _9.HImage2BitmapImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filepath = string.Empty;
            BitmapImage bitmapImage = new BitmapImage();
            ///注意.Net 5下操作的不同
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "C:\\Users\\LFLFLF\\Desktop\\";
            openFileDialog.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tiff;";
            openFileDialog.RestoreDirectory = true;


            if (openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;
                ///读文件的一种方式，暂不使用
                //{
                //    var fileStream = openFileDialog.OpenFile();

                //    using (StreamReader reader = new StreamReader(fileStream))
                //    {
                //       var filecontent = reader.ReadToEnd();
                //    }
                //}

                using (FileStream stream = new FileStream(filepath, FileMode.Open))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }

                //Img1.Source = bitmapImage;

                int imageTest_Height = 0;

                ///从文件读取到内存流中
                Stream imageStream = new MemoryStream();
                byte[] imageData = File.ReadAllBytes(filepath);
                imageStream.Write(imageData, 0, imageData.Length);

                ///注意.Net 5引用的是System.Drawing.Common的nuget包老版本
                Bitmap bitmap = new Bitmap(imageStream);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
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
                    imageTest_Height = hv_img_h.I;
                    ho_Monkey.Dispose();
                    ho_Region.Dispose();

                    hv_Height.Dispose();
                    hv_Width.Dispose();
                    hv_Ratio.Dispose();
                    hv_img_h.Dispose();

                    hImage.Dispose();

                }

                Debug.WriteLine(imageTest_Height);
            }

        }

        //public Bitmap ConvertHObjectToBitmap(HObject hObject)
        //{
        //    HTuple width, height;
        //    HOperatorSet.GetImageSize(hObject, out width, out height);

        //    IntPtr ptr = hObject.GetIntPtr();

        //    Bitmap bitmap = new Bitmap(width, height, width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ptr);

        //    return bitmap;
        //}

        /// <summary>
        /// 灰度图像 HObject -> HImage1
        /// </summary>
        public HImage HObject2HImage1(HObject hObj)
        {
            HImage image = new HImage();
            HTuple type, width, height, pointer;
            HOperatorSet.GetImagePointer1(hObj, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
            return image;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            // 读取图像
            HImage hImage = new HImage(@"pexels-francesco-ungaro-1525041.bmp");

            // 获取存放r，g，b值的指针
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            hImage.GetImagePointer3(out IntPtr r, out IntPtr g, out IntPtr b, out string type, out int w, out int h);
            byte[] red = new byte[w * h];
            byte[] green = new byte[w * h];
            byte[] blue = new byte[w * h];
            // 将指针指向地址的值取出来放到byte数组中
            Marshal.Copy(r, red, 0, w * h);
            Marshal.Copy(g, green, 0, w * h);
            Marshal.Copy(b, blue, 0, w * h);
            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format32bppRgb);
            Rectangle rect = new Rectangle(0, 0, w, h);
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

            red = null;
            green = null;
            blue = null;

           // bitmap.Dispose();
           // bitmapData = null;
            bptr = IntPtr.Zero;

            stopwatch.Stop();

            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // 输出运行时间
            Debug.WriteLine("使用Marshal方法运行时间: " + elapsedTime.TotalMilliseconds);

            hImage.Dispose();

            GC.Collect();
        }

        private void pointer_test_Click(object sender, RoutedEventArgs e)
        {


            HImage hImage = new HImage(@"pexels-francesco-ungaro-1525041.bmp");
            // 读取图像
            // 获取存放r，g，b值的指针
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();

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
            BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            IntPtr intPtr = bitmapData2.Scan0;
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

            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // 输出运行时间
            Debug.WriteLine("使用unsafe Pointer方法运行时间: " + elapsedTime.TotalMilliseconds);

            hImage.Dispose();

        }

        private void gray_pointer_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"1.bmp");
            IntPtr intPtr = hImage.GetImagePointer1(out string type, out int width, out int height);
            byte[] buffer = new byte[width * height];
            Marshal.Copy(intPtr, buffer, 0, buffer.Length);
            Bitmap bitmap3 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            Rectangle rect3 = new Rectangle(0, 0, width, height);
            BitmapData bitmapData3 = bitmap3.LockBits(rect3, ImageLockMode.ReadWrite, bitmap3.PixelFormat);
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
            //bitmap3.UnlockBits(bitmapData3);

            bitmap3.Save(@"2.bmp");

            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;
            // 输出运行时间
            Debug.WriteLine("使用unsafe Pointer方法运行时间: " + elapsedTime.TotalMilliseconds);

            hImage.Dispose();
        }

        private void gray_pointer2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
