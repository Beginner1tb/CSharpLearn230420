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
            // BitmapData bitmapData3 = bitmap3.LockBits(rect3, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
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

            // bitmap3.UnlockBits(bitmapData3);


            // 获取经过的时间
            TimeSpan elapsedTime = stopwatch.Elapsed;
            // 输出运行时间
            Debug.WriteLine("使用unsafe Pointer方法运行时间: " + elapsedTime.TotalMilliseconds);
            bitmap3.Save(@"2.bmp");
            hImage.Dispose();
        }


        /// <summary>
        /// 暂时不用
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
            Bitmap bitmap = HImageToBitmap(hImage);


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
		/// Halcon Image .NET Bitmap
		/// </summary>
		/// <param name="halconImage"></param>
		/// <returns></returns>
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

        public static Bitmap HObject2Bitmap24(HObject hObject)
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

        private void hobject_convert_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            // 启动计时器
            stopwatch.Start();
            HImage hImage = new HImage(@"1.bmp");

            string filepath = @"1.bmp";

            HObject hObject = ReadFileToHObject(filepath);


            Bitmap bitmap;
            HObject2Bitmap(hObject, out bitmap);


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


            Bitmap bitmap = HObject2Bitmap24(hObject);



            TimeSpan elapsedTime = stopwatch.Elapsed;
            bitmap.Save(@"hobject_convert_color.bmp",ImageFormat.Bmp);
            // 输出运行时间
            Debug.WriteLine("hobject_conver彩色运行时间: " + elapsedTime.TotalMilliseconds);

            hObject.Dispose();
            bitmap.Dispose();
            hImage.Dispose();
        }
    }
}
