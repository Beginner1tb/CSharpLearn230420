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

namespace _11.ROItoModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isDragging = false;
        private Point startPoint_1;
        private FrameworkElement selectedRect;

        string filepath;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void topRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(topRect);
            isDragging = true;
            selectedRect = topRect;
            // Canvas.SetTop(bottomRect, Canvas.GetTop(topRect) + roiRect_1.Height-5);
            ((System.Windows.Shapes.Rectangle)sender).CaptureMouse();

        }

        private void bottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(bottomRect);
            isDragging = true;
            selectedRect = bottomRect;
            bottomRect.CaptureMouse();
        }

        private void leftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(leftRect);
            isDragging = true;
            selectedRect = leftRect;
            leftRect.CaptureMouse();
        }

        private void rightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(rightRect);
            isDragging = true;
            selectedRect = rightRect;
            rightRect.CaptureMouse();
        }



        private void topRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == topRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                //double top = currentPosition.Y - startPoint_1.Y;
                startPoint_1.Y = Canvas.GetTop(topRect);

                double deltaY = startPoint_1.Y - currentPosition.Y;
                if (currentPosition.Y <= 0)
                {
                    currentPosition.Y = 0;
                    deltaY = 0;
                }
                else
                {
                    if (Canvas.GetTop(bottomRect) - currentPosition.Y <= 20)
                    {
                        currentPosition.Y = Canvas.GetTop(bottomRect) - 20;
                    }
                }



                double top = currentPosition.Y;

                double newHeight = roiRect_1.Height + deltaY;
                if (newHeight > 20)
                {
                    roiRect_1.Height = newHeight;
                }
                else
                {
                    roiRect_1.Height = 20;
                }

                Canvas.SetTop(roiRect_1, top);
                Canvas.SetTop(topRect, top);
                Canvas.SetTop(leftRect, top);
                Canvas.SetTop(rightRect, top);


                //设置框体代号位置

                Canvas.SetTop(tb_rect1, top - 35);

            }
        }

        private void bottomRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == bottomRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                //double top = currentPosition.Y - startPoint_1.Y;
                startPoint_1.Y = Canvas.GetTop(bottomRect);
                double deltaY = currentPosition.Y - startPoint_1.Y;
                if (currentPosition.Y >= canvas_1.ActualHeight)
                {
                    currentPosition.Y = canvas_1.ActualHeight;
                    deltaY = 0;
                }
                else
                {
                    if (currentPosition.Y - Canvas.GetTop(topRect) <= 20)
                    {
                        currentPosition.Y = Canvas.GetTop(topRect) + 20;
                    }
                }



                double bottom = currentPosition.Y;

                double newHeight = roiRect_1.Height + deltaY;
                if (newHeight > 20)
                {
                    roiRect_1.Height = newHeight;
                }
                else
                {
                    roiRect_1.Height = 20;
                }

                //Canvas.SetTop(roiRect_1, bottom);
                Canvas.SetTop(bottomRect, bottom);
                Canvas.SetTop(leftRect, Canvas.GetTop(topRect));
                Canvas.SetTop(rightRect, Canvas.GetTop(topRect));
            }
        }

        private void leftRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == leftRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                //double top = currentPosition.Y - startPoint_1.Y;
                startPoint_1.X = Canvas.GetLeft(leftRect);

                double deltaX = startPoint_1.X - currentPosition.X;
                if (currentPosition.X <= 0)
                {
                    currentPosition.X = 0;
                    deltaX = 0;
                }
                else
                {
                    if (Canvas.GetLeft(rightRect) - currentPosition.X <= 20)
                    {
                        currentPosition.X = Canvas.GetLeft(rightRect) - 20;
                    }
                }



                double left = currentPosition.X;

                double newWidth = roiRect_1.Width + deltaX;
                if (newWidth > 20)
                {
                    roiRect_1.Width = newWidth;
                }
                else
                {
                    roiRect_1.Width = 20;
                }

                Canvas.SetLeft(roiRect_1, left);
                Canvas.SetLeft(leftRect, left);
                Canvas.SetLeft(topRect, left);
                Canvas.SetLeft(bottomRect, left);

                //设置框体代号位置

                Canvas.SetLeft(tb_rect1, left);
                // Canvas.SetTop(tb_rect1, marginTop - 35);
            }
        }

        private void rightRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == rightRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                //double top = currentPosition.Y - startPoint_1.Y;
                startPoint_1.X = Canvas.GetLeft(rightRect);
                double deltaX = currentPosition.X - startPoint_1.X;
                if (currentPosition.X >= canvas_1.ActualWidth)
                {
                    currentPosition.X = canvas_1.ActualWidth;
                    deltaX = 0;
                }
                else
                {
                    if (currentPosition.X - Canvas.GetLeft(leftRect) <= 20)
                    {
                        currentPosition.X = Canvas.GetLeft(leftRect) + 20;
                    }
                }



                double right = currentPosition.X;

                double newWidth = roiRect_1.Width + deltaX;
                if (newWidth > 20)
                {
                    roiRect_1.Width = newWidth;
                }
                else
                {
                    roiRect_1.Width = 20;
                }

                //Canvas.SetLeft(roiRect_1, bottom);
                Canvas.SetLeft(rightRect, right);
                Canvas.SetLeft(topRect, Canvas.GetLeft(leftRect));
                Canvas.SetLeft(bottomRect, Canvas.GetLeft(leftRect));
            }
        }


        private void topRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            topRect.ReleaseMouseCapture();
        }

        private void bottomRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            bottomRect.ReleaseMouseCapture();
        }

        private void leftRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            leftRect.ReleaseMouseCapture();
        }

        private void rightRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            rightRect.ReleaseMouseCapture();
        }

        private void roiRect_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(roiRect_1);
            isDragging = true;
            selectedRect = roiRect_1;
            roiRect_1.CaptureMouse();
        }


        private void roiRect_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == roiRect_1)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                double left = currentPosition.X - startPoint_1.X;
                double top = currentPosition.Y - startPoint_1.Y;

                //计算方块左上角的位置

                double marginLeft = currentPosition.X - roiRect_1.Width / 2;
                double marginTop = currentPosition.Y - roiRect_1.Height / 2;

                if (marginLeft < 0)
                {
                    marginLeft = 0;

                }
                else if ((marginLeft + roiRect_1.Width) > canvas_1.ActualWidth)
                {
                    marginLeft = canvas_1.ActualWidth - roiRect_1.Width;
                }

                if (marginTop < 0)
                {
                    marginTop = 0;

                }
                else if ((marginTop + roiRect_1.Height) > canvas_1.ActualHeight)
                {
                    marginTop = canvas_1.ActualHeight - roiRect_1.Height;
                }



                Canvas.SetLeft(roiRect_1, marginLeft);
                Canvas.SetTop(roiRect_1, marginTop);

                Canvas.SetLeft(topRect, marginLeft);
                Canvas.SetTop(topRect, marginTop);

                Canvas.SetLeft(leftRect, marginLeft);
                Canvas.SetTop(leftRect, marginTop);

                Canvas.SetLeft(bottomRect, marginLeft);
                Canvas.SetTop(bottomRect, marginTop + roiRect_1.Height - 3);


                Canvas.SetLeft(rightRect, marginLeft + roiRect_1.Width - 3);
                Canvas.SetTop(rightRect, marginTop);


                //设置框体代号位置

                Canvas.SetLeft(tb_rect1, marginLeft);
                Canvas.SetTop(tb_rect1, marginTop - 35);
            }
        }

        private void roiRect_1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            roiRect_1.ReleaseMouseCapture();
        }

        private void SaveRect2_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(img1.Width);
            Debug.WriteLine(img1.Height);
            ImageSource imageSource = img1.Source;
            string imagePath = "D://TestFolder//Image//front//Orignal-z.bmp";
            Uri imageUri = new Uri(System.IO.Path.GetFullPath(imagePath));

            BitmapImage bitmapImage = new BitmapImage(imageUri);
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            // 使用获取到的宽度和高度进行后续处理
            Debug.WriteLine("Image Width: " + width);
            Debug.WriteLine("Image Height: " + height);
            MessageBox.Show($"左上角X偏移：{Canvas.GetLeft(roiRect_1) * width / img1.Width}\n" +
                $"左上角Y偏移：{Canvas.GetTop(roiRect_1) * height / img1.Height}\n" +
                $"右下角X偏移：{(Canvas.GetLeft(roiRect_1) + roiRect_1.Width) * width / img1.Width}\n" +
                $"右下角Y偏移：{(Canvas.GetTop(roiRect_1) + roiRect_1.Height) * height / img1.Height}", "结果");
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
                        // Local iconic variables 

                        //HObject ho_OrignalZ, ho_ROI_0, ho_ImageReduced;
                        //HObject ho_ImagePart;
                        //// Initialize local and output iconic variables 
                        //HOperatorSet.GenEmptyObj(out ho_OrignalZ);
                        //HOperatorSet.GenEmptyObj(out ho_ROI_0);
                        //HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                        //HOperatorSet.GenEmptyObj(out ho_ImagePart);
                        //ho_OrignalZ.Dispose();
                        //HOperatorSet.GenImage1(out ho_OrignalZ, "byte", bitmap_init.Width, bitmap_init.Height, bitmapData.Scan0);
                        //ho_ROI_0.Dispose();
                        //HOperatorSet.GenRectangle1(out ho_ROI_0, Canvas.GetTop(roiRect_1) * 5120 / 400, 
                        //    Canvas.GetLeft(roiRect_1) * 5120 / 400,
                        //    (Canvas.GetTop(roiRect_1) + roiRect_1.Height) * 5120 / 400,
                        //    (Canvas.GetLeft(roiRect_1) + roiRect_1.Width) * 5120 / 400);
                        ////HOperatorSet.GenRectangle1(out ho_ROI_0, 1407.4, 1942.74, 1695.17, 2306.79);
                        //ho_ImageReduced.Dispose();
                        //HOperatorSet.ReduceDomain(ho_OrignalZ, ho_ROI_0, out ho_ImageReduced);
                        //ho_ImagePart.Dispose();
                        //HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                        //HOperatorSet.WriteImage(ho_ImagePart, "bmp", 0, "C:/Users/LFLFLF/Desktop/crop.bmp");
                        //ho_OrignalZ.Dispose();
                        //ho_ROI_0.Dispose();
                        //ho_ImageReduced.Dispose();
                        //ho_ImagePart.Dispose();
                    }
                    bitmap_init.UnlockBits(bitmapData);
                }
                bitmap_init.Dispose();
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
                Bitmap bitmap_init = new Bitmap(filepath);
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
                        // Local iconic variables 

                        HObject ho_OrignalZ, ho_ROI_0, ho_ImageReduced;
                        HObject ho_ImagePart;


                        // Local control variables 
                        HTuple hv_ModelID = new HTuple();

                        // Initialize local and output iconic variables 


                        HOperatorSet.GenEmptyObj(out ho_OrignalZ);
                        HOperatorSet.GenEmptyObj(out ho_ROI_0);
                        HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                        HOperatorSet.GenEmptyObj(out ho_ImagePart);
                        ho_OrignalZ.Dispose();
                        HOperatorSet.GenImage1(out ho_OrignalZ, "byte", bitmap_init.Width, bitmap_init.Height, bitmapData.Scan0);
                        ho_ROI_0.Dispose();
                        HOperatorSet.GenRectangle1(out ho_ROI_0, Canvas.GetTop(roiRect_1) * 5120 / img1.Height,
                            Canvas.GetLeft(roiRect_1) * 5120 / img1.Width,
                            (Canvas.GetTop(roiRect_1) + roiRect_1.Height) * 5120 / img1.Height,
                            (Canvas.GetLeft(roiRect_1) + roiRect_1.Width) * 5120 / img1.Width);
                        //HOperatorSet.GenRectangle1(out ho_ROI_0, 1407.4, 1942.74, 1695.17, 2306.79);
                        ho_ImageReduced.Dispose();
                        HOperatorSet.ReduceDomain(ho_OrignalZ, ho_ROI_0, out ho_ImageReduced);
                        ho_ImagePart.Dispose();
                        HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
                        HOperatorSet.WriteImage(ho_ImagePart, "bmp", 0, "C:/Users/LFLFLF/Desktop/crop.bmp");
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ModelID.Dispose();
                            HOperatorSet.CreateShapeModel(ho_ImagePart, 4, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad()
                                , "auto", "auto", "use_polarity", 30, 0, out hv_ModelID);
                        }
                        HOperatorSet.WriteShapeModel(hv_ModelID, "C:/Users/LFLFLF/Desktop/crop.shm");


                        HOperatorSet.ClearShapeModel(hv_ModelID);
                        ho_OrignalZ.Dispose();
                        ho_ROI_0.Dispose();
                        ho_ImageReduced.Dispose();
                        ho_ImagePart.Dispose();

                        hv_ModelID.Dispose();
                    }
                    bitmap_init.UnlockBits(bitmapData);
                }
                bitmap_init.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
