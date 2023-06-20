using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Point = System.Windows.Point;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace _12.ROIforImageLocation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filepath;
        HImage hImage1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            ///注意.Net 5下操作的不同
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = "D:\\TestFolder\\Image\\front\\",
                Filter = "图像文件|*.bmp",
                RestoreDirectory = true,
            };


            if (openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;

                Bitmap bitmap_init = new Bitmap(filepath);
                ///调整显示图像的大小
                //Bitmap bitmapResize = new Bitmap(bitmap_init, 600, 600);
                Bitmap bitmap_resize = BitmapResize(bitmap_init);

                img1.Source = ImageControlShow(bitmap_resize);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap_init.Width, bitmap_init.Height);
                if (IsColorBitmap(bitmap_init))
                {
                    BitmapData bitmapData = bitmap_init.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    {
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
                bitmap_init.Dispose();
                bitmap_resize.Dispose();
            }
        }


        public Bitmap BitmapResize(Bitmap bitmap)
        {
            Bitmap bitmapResize = new Bitmap(bitmap, 400, 400);
            return bitmapResize;
        }

        bool IsColorBitmap(Bitmap bitmap)
        {
            PixelFormat format = bitmap.PixelFormat;
            return format == PixelFormat.Format32bppArgb || format == PixelFormat.Format32bppRgb
                || format == PixelFormat.Format24bppRgb || format == PixelFormat.Format32bppPArgb;
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

        private void CropImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                hImage1 = new HImage();
                Bitmap bitmap_init = new Bitmap(filepath);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap_init.Width, bitmap_init.Height);
                BitmapData bitmapData = bitmap_init.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                {
                    // Local iconic variables 
              
                    HObject ho_OrignalZ, ho_Region, ho_RegionOpening = null;
                    HObject ho_RegionClosing = null, ho_RegionFillUp2 = null, ho_ConnectedRegions = null;
                    HObject ho_SelectedRegions = null, ho_Contours = null, ho_ContCircle = null;
                    HObject ho_Region5 = null, ho_ImagePart = null;

                    // Local control variables 

                    HTuple hv_locate_retract_dis = new HTuple();
                    HTuple hv_coin_locate_thr = new HTuple(), hv_coin_locate_open_ker = new HTuple();
                    HTuple hv_coin_locate_clo_ker = new HTuple(), hv_Area = new HTuple();
                    HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
                    HTuple hv_NoCoin = new HTuple(), hv_Row3 = new HTuple();
                    HTuple hv_Column3 = new HTuple(), hv_Radius = new HTuple();
                    HTuple hv_StartPhi = new HTuple(), hv_EndPhi = new HTuple();
                    HTuple hv_PointOrder = new HTuple(), hv_Row1 = new HTuple();
                    HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
                    HTuple hv_Column2 = new HTuple();
                    // Initialize local and output iconic variables 
                    HOperatorSet.GenEmptyObj(out ho_OrignalZ);
                    HOperatorSet.GenEmptyObj(out ho_Region);
                    HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                    HOperatorSet.GenEmptyObj(out ho_RegionClosing);
                    HOperatorSet.GenEmptyObj(out ho_RegionFillUp2);
                    HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                    HOperatorSet.GenEmptyObj(out ho_Contours);
                    HOperatorSet.GenEmptyObj(out ho_ContCircle);
                    HOperatorSet.GenEmptyObj(out ho_Region5);
                    HOperatorSet.GenEmptyObj(out ho_ImagePart);
                    ho_OrignalZ.Dispose();
                    HOperatorSet.GenImage1(out ho_OrignalZ, "byte", bitmap_init.Width, bitmap_init.Height, bitmapData.Scan0);

                    //硬币定位算法

                    //*定位参数
                    //定位参数：定位缩进
                    hv_locate_retract_dis.Dispose();
                    hv_locate_retract_dis = 110;
                    //定位参数：定位阈值
                    hv_coin_locate_thr.Dispose();
                    hv_coin_locate_thr = 110;
                    //定位参数：定位开核
                    hv_coin_locate_open_ker.Dispose();
                    hv_coin_locate_open_ker = 3.5;
                    //定位参数：定位闭核
                    hv_coin_locate_clo_ker.Dispose();
                    hv_coin_locate_clo_ker = 7.5;


                    hv_locate_retract_dis = int.Parse(Param_locate_retract_dis.Text);
                    hv_coin_locate_thr = int.Parse(param_coin_locate_thr.Text); ;
                    hv_coin_locate_open_ker = float.Parse(Param_coin_locate_open_ker.Text);
                    hv_coin_locate_clo_ker = float.Parse(Param_coin_locate_clo_ker.Text);

                    //*******定位算法***********
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_OrignalZ, out ho_Region, hv_coin_locate_thr, 255);
                    hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                    HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                    if ((int)((new HTuple((new HTuple(hv_Area.TupleLength())).TupleEqual(0))).TupleOr(
                        new HTuple(hv_Area.TupleLess(9999)))) != 0)
                    {
                        hv_NoCoin.Dispose();
                        hv_NoCoin = 1;
                    }
                    else
                    {
                        ho_RegionOpening.Dispose();
                        HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, hv_coin_locate_open_ker);
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingCircle(ho_RegionOpening, out ho_RegionClosing, hv_coin_locate_clo_ker);
                        ho_RegionFillUp2.Dispose();
                        HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp2);
                        ho_ConnectedRegions.Dispose();
                        HOperatorSet.Connection(ho_RegionFillUp2, out ho_ConnectedRegions);
                        ho_SelectedRegions.Dispose();
                        HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out ho_SelectedRegions, "max_area",
                            0);
                        ho_Contours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_SelectedRegions, out ho_Contours, "border");
                        hv_Row3.Dispose(); hv_Column3.Dispose(); hv_Radius.Dispose(); hv_StartPhi.Dispose(); hv_EndPhi.Dispose(); hv_PointOrder.Dispose();
                        HOperatorSet.FitCircleContourXld(ho_Contours, "geotukey", -1, 0, 0, 3, 2, out hv_Row3,
                            out hv_Column3, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                        ho_ContCircle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_Row3, hv_Column3, hv_Radius,
                            hv_StartPhi, hv_EndPhi, "positive", 1);
                        ho_Region5.Dispose();
                        HOperatorSet.GenRegionContourXld(ho_ContCircle, out ho_Region5, "filled");
                        hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                        HOperatorSet.SmallestRectangle1(ho_Region5, out hv_Row1, out hv_Column1, out hv_Row2,
                            out hv_Column2);
                        //截图预留根据效果调整
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ImagePart.Dispose();
                            HOperatorSet.CropRectangle1(ho_OrignalZ, out ho_ImagePart, hv_Row1 - hv_locate_retract_dis,
                                hv_Column1 - hv_locate_retract_dis, hv_Row2 + hv_locate_retract_dis, hv_Column2 + hv_locate_retract_dis);



                        }

                      // HObject2Bitmap(ho_ImagePart, out Bitmap bmp_ImageMead);
         

                        //HObject2Himage(ho_ImagePart, ref hImage1);

                        hv_NoCoin.Dispose();
                        hv_NoCoin = 0;


                    }

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    hImage1 = HObject2Himage(ho_ImagePart);
                    Bitmap bmp_ImageMead = HImageToBitmap(hImage1);
                    img1.Source = ImageControlShow(bmp_ImageMead);
                    TimeSpan timeSpan = stopwatch.Elapsed;
                    Debug.WriteLine("himage" + timeSpan.TotalMilliseconds);
                    
                   
                    bmp_ImageMead.Dispose();
                   
                    ho_OrignalZ.Dispose();
                    ho_Region.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionClosing.Dispose();
                    ho_RegionFillUp2.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();
                    ho_Contours.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Region5.Dispose();
                    ho_ImagePart.Dispose();

                    hv_locate_retract_dis.Dispose();
                    hv_coin_locate_thr.Dispose();
                    hv_coin_locate_open_ker.Dispose();
                    hv_coin_locate_clo_ker.Dispose();
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    hv_NoCoin.Dispose();
                    hv_Row3.Dispose();
                    hv_Column3.Dispose();
                    hv_Radius.Dispose();
                    hv_StartPhi.Dispose();
                    hv_EndPhi.Dispose();
                    hv_PointOrder.Dispose();
                    hv_Row1.Dispose();
                    hv_Column1.Dispose();
                    hv_Row2.Dispose();
                    hv_Column2.Dispose();
                   

                }
                bitmap_init.UnlockBits(bitmapData);
                bitmap_init.Dispose();
                hImage1.Dispose();
                
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void HObject2Bitmap(HObject image, out Bitmap bitmap)
        {
            HTuple hpoint, type, width, height;
            const int Alpha = 255;
            
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);

            bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //bitmap = new Bitmap(3096, 3096, PixelFormat.Format8bppIndexed);
            ColorPalette pal = bitmap.Palette;
            for (int i = 0; i < 255; i++)
            {
                pal.Entries[i] = System.Drawing.Color.FromArgb(Alpha, i, i, i);
            }
            bitmap.Palette = pal;
            //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
            //BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 3096, 3096);
            //BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            //int s = bitmapData.Stride;
            //int pixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            //IntPtr intPtr1 = bitmapData.Scan0;
            //IntPtr intPtr2 = hpoint;


            int iPadding = (4 - (width % 4)) % 4;
            int iStride = width + iPadding;
            //
            // 3) Create a new gray Bitmap object, allocating the necessary (managed) memory 
          //  bitmap = new Bitmap(width, widthD, PixelFormat.Format8bppIndexed);
            // note for high performance: in case of padding=0, image can be copied by reference.
            // however, then the bitmap's validity relies on the HImage lifetime.
            // bitmap = new Bitmap(iWidth, iHeight, iWidth, PixelFormat.Format8bppIndexed, ip_Gray);
            //
            // 4) Copy the image data directly into the bitmap data object, re-arranged in the required bitmap order
            // BitmapData lets us access the data in memory
            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, bitmap.PixelFormat);
            // System.Threading.Tasks.Parallel processing requires .NET framework >= 4.0 
            Parallel.For(0, height, r =>
            {
                IntPtr posRead = hpoint + r * width;
                IntPtr posWrite = bmpData.Scan0 + r * iStride;
                for (int c = 0; c < width; c++)
                    Marshal.WriteByte((IntPtr)posWrite, c, Marshal.ReadByte((IntPtr)posRead, c));
            });


            bitmap.UnlockBits(bmpData);
            
            GC.Collect();
        }

        public static Bitmap HImage2Bitmap8Ptr(HImage hImage)
        {
            try
            {
                IntPtr intPtr = hImage.GetImagePointer1(out string type, out int width, out int height);
                byte[] buffer = new byte[width * height];
                Marshal.Copy(intPtr, buffer, 0, buffer.Length);
                Bitmap bitmap3 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                System.Drawing.Rectangle rect3 = new System.Drawing.Rectangle(0, 0, width, height);
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
                return bitmap3;
            }
            catch (Exception ex)
            {

                throw new Exception("HImage转8BMP错误", ex);
            }
        }

        public HImage HObject2Himage(HObject hObject)
        {
            HImage hImage = new HImage();
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hObject, out pointer, out type, out width, out height);
            hImage.GenImage1(type, width, height, pointer);
            return hImage;
        }

        //public void HObject2Bpp24(HObject ho_image, out Bitmap res24)
        //{
        //    //HOperatorSet.WriteImage(ho_image, "bmp", 0, @"E:\Dick\CASE\Type C R1.3&0.9\data\20200520\NG原图\CCD1\ex\xiaok1.bmp");
        //    HTuple width0, height0, type, width, height;
        //    //获取图像尺寸
        //    HOperatorSet.GetImageSize(ho_image, out width0, out height0);
        //    HOperatorSet.InterleaveChannels(ho_image, out HObject InterImage, "argb", "match", 255);  //"rgb", 4 * width0, 0     "argb", "match", 255
        //    //获取交错格式图像指针
        //    HOperatorSet.GetImagePointer1(InterImage, out HTuple Pointer, out type, out width, out height);
        //    IntPtr ptr = Pointer;
        //    //构建新Bitmap图像
        //    Bitmap res32 = new Bitmap(width / 4, height, width, PixelFormat.Format32bppArgb, ptr);  // Format32bppArgb     Format24bppRgb
        //    //32位Bitmap转24位
        //    res24 = new Bitmap(res32.Width, res32.Height, PixelFormat.Format24bppRgb);
        //    Graphics graphics = Graphics.FromImage(res24);
        //    graphics.DrawImage(res32, new Rectangle(0, 0, res32.Width, res32.Height));
        //    res32.Dispose();
        //}
        //public void HObject2Bpp8(HObject ho_image, out Bitmap res)
        //{
        //    HTuple hpoint, type, width, height;
        //    const int Alpha = 255;
        //    HOperatorSet.GetImagePointer1(ho_image, out hpoint, out type, out width, out height);
        //    res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
        //    ColorPalette pal = res.Palette;
        //    for (int i = 0; i <= 255; i++)
        //    {
        //        pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
        //    }
        //    res.Palette = pal;
        //    Rectangle rect = new Rectangle(0, 0, width, height);
        //    BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
        //    int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
        //    IntPtr ptr1 = bitmapData.Scan0;
        //    IntPtr ptr2 = hpoint;
        //    int bytes = width * height;
        //    byte[] rgbvalues = new byte[bytes];
        //    System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbvalues, 0, bytes);
        //    System.Runtime.InteropServices.Marshal.Copy(rgbvalues, 0, ptr1, bytes);
        //    res.UnlockBits(bitmapData);
        //}

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
                    BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, iWidth, iHeight),
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
                    BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, iWidth, iHeight),
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
