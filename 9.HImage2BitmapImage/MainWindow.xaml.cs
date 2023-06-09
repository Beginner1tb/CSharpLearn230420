﻿using HalconDotNet;
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
            //hImage.GetImagePointer3(out IntPtr r, out IntPtr g, out IntPtr b, out string type, out int w, out int h);
            //byte[] red = new byte[w * h];
            //byte[] green = new byte[w * h];
            //byte[] blue = new byte[w * h];
            //// 将指针指向地址的值取出来放到byte数组中
            //Marshal.Copy(r, red, 0, w * h);
            //Marshal.Copy(g, green, 0, w * h);
            //Marshal.Copy(b, blue, 0, w * h);
            //// Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format32bppRgb);
            //Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            //Rectangle rect = new Rectangle(0, 0, w, h);
            //BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            ////  BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            //IntPtr bptr = bitmapData.Scan0;
            //for (int i = 0; i < red.Length; i++)
            //{
            //    Marshal.Copy(blue, i, bptr + i * 4, 1);
            //    Marshal.Copy(green, i, bptr + i * 4 + 1, 1);
            //    Marshal.Copy(red, i, bptr + i * 4 + 2, 1);
            //    // Marshal.Copy(new byte[] { 255 }, 0, bptr + i * 4 + 3, 1);
            //}

            //bitmap.UnlockBits(bitmapData);

            // bitmap.Dispose();
            // bitmapData = null;

            Bitmap bitmap = HImageMethod_Marshal.HImage2Bitmap24Intptr(hImage);

            stopwatch.Stop();

            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;
            bitmap.Save("method1.bmp", ImageFormat.Bmp);
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

            //hImage.GetImagePointer3(out IntPtr r, out IntPtr g, out IntPtr b, out string type, out int w, out int h);
            //byte[] red = new byte[w * h];
            //byte[] green = new byte[w * h];
            //byte[] blue = new byte[w * h];
            //// 将指针指向地址的值取出来放到byte数组中
            //Marshal.Copy(r, red, 0, w * h);
            //Marshal.Copy(g, green, 0, w * h);
            //Marshal.Copy(b, blue, 0, w * h);
            //Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            ////Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format32bppRgb);
            //Rectangle rect2 = new Rectangle(0, 0, w, h);
            //BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            ////BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            //IntPtr intPtr = bitmapData2.Scan0;

            //int iPadding = (4 - ((w * 3) % 4)) % 4;
            //int iStride = w * 3 + iPadding;
            ////
            //unsafe
            //{

            //    byte* bptr2 = (byte*)bitmapData2.Scan0;
            //    //for (int i = 0; i < w * h; i++)
            //    //{
            //    //    bptr2[i * 4] = blue[i];
            //    //    bptr2[i * 4 + 1] = green[i];
            //    //    bptr2[i * 4 + 2] = red[i];
            //    //    bptr2[i * 4 + 3] = 255;
            //    //}

            //    for (int i = 0; i < w * h; i++)
            //    {
            //        bptr2[i * 3] = blue[i];
            //        bptr2[i * 3 + 1] = green[i];
            //        bptr2[i * 3 + 2] = red[i];

            //    }



            //}
            //bitmap2.UnlockBits(bitmapData2);

            Bitmap bitmap2 = HImageMethod_unsafePtr.HImage2Bitmap24Ptr(hImage);


            bitmap2.Save("method2.bmp", ImageFormat.Bmp);
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
            Bitmap bitmap3 = HImageMethod_unsafePtr.HImage2Bitmap8Ptr(hImage);
            // bitmap3.UnlockBits(bitmapData3);


            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;
            // 输出运行时间
            Debug.WriteLine("使用unsafe Pointer方法运行时间: " + elapsedTime.TotalMilliseconds);
            bitmap3.Save(@"2.bmp");
            hImage.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gray_pointer2_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"1.bmp");
            string filepath = @"pexels-francesco-ungaro-1525041.bmp";

            //HObject hObject = ReadFileToHObject(filepath);
            HObject hObject = ConvertHImageToHObject(hImage);
            //Bitmap bitmap = ConvertHalconImageToBitmap(hObject, true);
            Bitmap bitmap = HImageMethod_Parallel.HImageToBitmap(hImage);


            TimeSpan elapsedTime = stopwatch.Elapsed;
            bitmap.Save(@"hobject.bmp");
            // 输出运行时间
            Debug.WriteLine("使用Intptr方法运行时间: " + elapsedTime.TotalMilliseconds);

            hObject.Dispose();

            // GC.Collect();
        }

        // 将HImage转换为HObject
        public HObject ConvertHImageToHObject(HImage himage)
        {
            HObject hObject;
            HOperatorSet.GenEmptyObj(out hObject);
            HOperatorSet.ConvertImageType(himage, out hObject, "byte");

            return hObject;
        }


        // 从文件地址读取到HObject
        public HObject ReadFileToHObject(string filepath)
        {
            HObject hObject;
            HOperatorSet.GenEmptyObj(out hObject);
            HOperatorSet.ReadImage(out hObject, filepath);

            return hObject;
        }


        /// <summary>
        /// Halcon Image .NET Bitmap，内存问题，不可用
        /// </summary>
        /// <param name="halconImage"></param>
        /// <returns></returns>
        [Obsolete("Has problem in memory copy")]
        public static Bitmap ConvertHalconImageToBitmap(HObject halconImage, bool isColor)
        {
            if (halconImage == null)
            {
                throw new ArgumentNullException("halconImage");
            }

            HTuple pointerRed = null;
            HTuple pointerGreen = null;
            HTuple pointerBlue = null;
            HTuple type;
            HTuple width;
            HTuple height;

            // Halcon
            var pixelFormat = (isColor) ? PixelFormat.Format32bppRgb : PixelFormat.Format8bppIndexed;
            if (isColor)
                HOperatorSet.GetImagePointer3(halconImage, out pointerRed, out pointerGreen, out pointerBlue, out type, out width, out height);
            else
                HOperatorSet.GetImagePointer1(halconImage, out pointerBlue, out type, out width, out height);


            Bitmap bitmap = new Bitmap((Int32)width, (Int32)height, pixelFormat);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            {
                int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
                byte[] rgbValues = new byte[bytes];

                lock (rgbValues)
                {
                    IntPtr ptrB = new IntPtr(pointerBlue);
                    IntPtr ptrG = IntPtr.Zero;
                    IntPtr ptrR = IntPtr.Zero;
                    if (pointerGreen != null) ptrG = new IntPtr(pointerGreen);
                    if (pointerRed != null) ptrR = new IntPtr(pointerRed);
                    int channels = (isColor) ? 3 : 1;

                    // Stride
                    int strideTotal = Math.Abs(bmpData.Stride);
                    int unmapByes = strideTotal - ((int)width * channels);
                    for (int i = 0, offset = 0; i < bytes; i += channels, offset++)
                    {
                        if ((offset + 1) % width == 0)
                        {
                            i += unmapByes;
                        }

                        rgbValues[i] = Marshal.ReadByte(ptrB, offset);
                        if (isColor)
                        {
                            rgbValues[i + 1] = Marshal.ReadByte(ptrG, offset);
                            rgbValues[i + 2] = Marshal.ReadByte(ptrR, offset);
                        }

                    }

                    Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
                }


            }

            bitmap.UnlockBits(bmpData);
            return bitmap;
        }







        private void hobject_convert_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"1.bmp");

            string filepath = @"1.bmp";

            HObject hObject = ReadFileToHObject(filepath);


            Bitmap bitmap;
            HObjectConvert_IntPtr.HObject2Bitmap(hObject, out bitmap);


            TimeSpan elapsedTime = stopwatch.Elapsed;
            bitmap.Save(@"hobject_convert.bmp");
            // 输出运行时间
            Debug.WriteLine("hobject_conver黑白运行时间: " + elapsedTime.TotalMilliseconds);

            hObject.Dispose();
            hImage.Dispose();
            bitmap.Dispose();
        }

        private void hobject_colorconvert_Click(object sender, RoutedEventArgs e)
        {

            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"color.bmp");

            string filepath = @"color.bmp";

            HObject hObject = ReadFileToHObject(filepath);


            // Bitmap bitmap = HObjectConvert_Halcon.HObject2Bitmap24(hObject);
            //  Bitmap bitmap = HObjectConvert_UnsafePtr.HObject2Bitmap24Ptr(hObject);

            //HImage hImage1 = new HImage();
            //hImage1 = HObjectToHImage.HObject2BitmapRGB(hObject);


            Bitmap bitmap = HObjectConvert_Halcon.HObject2Bitmap24(hObject);
            //Bitmap bitmap = HImageMethod_Parallel.HImageToBitmap(hImage1);


            TimeSpan elapsedTime = stopwatch.Elapsed;
            //   bitmap.Save(@"hobject_convert_color.bmp", ImageFormat.Bmp);
            //int size = Marshal.SizeOf(bitmap);
            // 输出运行时间
            Debug.WriteLine("hobject_conver彩色运行时间: " + elapsedTime.TotalMilliseconds);
            // Debug.WriteLine("bitmap大小" + size);
            hObject.Dispose();

            hImage.Dispose();

            BitmapImage bitmapImage;
            using (MemoryStream memoryStream = new MemoryStream())
            {

                bitmap.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            colorImage.Source = bitmapImage;

            //colorImage.Source = null;

            bitmap.Dispose();
            GC.Collect();
        }

        private void hobject_color_decompose_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"color.bmp");

            string filepath = @"color.bmp";

            HObject hObject = ReadFileToHObject(filepath);


            Bitmap bitmap = HObjectConvert_Decompose3.HObjectToBitmap(hObject);



            TimeSpan elapsedTime = stopwatch.Elapsed;
            bitmap.Save(@"hobject_convert_color_decompose.bmp", ImageFormat.Bmp);
            // 输出运行时间
            Debug.WriteLine("hobject_conver彩色运行时间: " + elapsedTime.TotalMilliseconds);

            hObject.Dispose();
            bitmap.Dispose();
            hImage.Dispose();
        }


    }
}
