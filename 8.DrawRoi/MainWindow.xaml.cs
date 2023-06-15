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

namespace _8.DrawRoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawing = false;
        private Point startPoint;


        private bool isDragging = false;
        private Point startPoint_1;
        private FrameworkElement selectedRect;


        //整体位移框用参数
        private bool isDragging_rec = false;
        private Point startPoint_rec;
        private Point initialPosition_rec;
        private Point offset_rec;

        public MainWindow()
        {
            InitializeComponent();
            //image.AddHandler(image.MouseLeftButtonDownEvent, new MouseEventHandler(image_MouseLeftButtonDown), true);
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(image);
            if (isDrawing != true)
            {
                isDrawing = true;
                roiRect.Visibility = Visibility.Visible;
            }
            else
            {
                //Point endPoint = e.GetPosition(image);

                //double left = Math.Min(startPoint.X, endPoint.X);
                //double top = Math.Min(startPoint.Y, endPoint.Y);
                //double width = Math.Abs(startPoint.X - endPoint.X);
                //double height = Math.Abs(startPoint.Y - endPoint.Y);

                //Canvas.SetLeft(roiRect, left);
                //Canvas.SetTop(roiRect, top);
                //roiRect.Width = width;
                //roiRect.Height = height;
                //roiRect.Visibility = Visibility.Visible;
                //Debug.WriteLine(DateTime.Now.ToString());
                //isDrawing = false;
            }

        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                Point endPoint = e.GetPosition(image);

                double left = Math.Min(startPoint.X, endPoint.X);
                double top = Math.Min(startPoint.Y, endPoint.Y);
                double width = Math.Abs(startPoint.X - endPoint.X);
                double height = Math.Abs(startPoint.Y - endPoint.Y);

                Canvas.SetLeft(roiRect, left);
                Canvas.SetTop(roiRect, top);
                roiRect.Width = width;
                roiRect.Height = height;
                roiRect.Visibility = Visibility.Visible;
                Debug.WriteLine(DateTime.Now.ToString());
                isDrawing = false;

                ((Image)image).ReleaseMouseCapture();

            }

        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                //if (e.LeftButton==MouseButtonState.Pressed)
                //{
                Point currentPosition = e.GetPosition(image);

                double left = Math.Min(startPoint.X, currentPosition.X);
                double top = Math.Min(startPoint.Y, currentPosition.Y);
                double width = Math.Abs(startPoint.X - currentPosition.X);
                double height = Math.Abs(startPoint.Y - currentPosition.Y);

                Canvas.SetLeft(roiRect, left);
                Canvas.SetTop(roiRect, top);
                roiRect.Width = width;
                roiRect.Height = height;
                //   Debug.WriteLine(isDrawing);
                //
                //if (e.LeftButton == MouseButtonState.Released)
                //{
                //    Point currentPosition = e.GetPosition(image);

                //    double left = Math.Min(startPoint.X, currentPosition.X);
                //    double top = Math.Min(startPoint.Y, currentPosition.Y);
                //    double width = Math.Abs(startPoint.X - currentPosition.X);
                //    double height = Math.Abs(startPoint.Y - currentPosition.Y);

                //    Canvas.SetLeft(roiRect, left);
                //    Canvas.SetTop(roiRect, top);
                //    roiRect.Width = width;
                //    roiRect.Height = height;
                //}

            }
        }

        private void image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void SaveRect1_Click(object sender, RoutedEventArgs e)
        {
            roiRect_1.Height = 20;
        }

        private void image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                Point endPoint = e.GetPosition(image);

                double left = Math.Min(startPoint.X, endPoint.X);
                double top = Math.Min(startPoint.Y, endPoint.Y);
                double width = Math.Abs(startPoint.X - endPoint.X);
                double height = Math.Abs(startPoint.Y - endPoint.Y);

                Canvas.SetLeft(roiRect, left);
                Canvas.SetTop(roiRect, top);
                roiRect.Width = width;
                roiRect.Height = height;
                roiRect.Visibility = Visibility.Visible;
                Debug.WriteLine(DateTime.Now.ToString());
                isDrawing = false;

                ((Image)image).ReleaseMouseCapture();

            }
        }




        private void topRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(topRect);
            isDragging = true;
            selectedRect = topRect;
            // Canvas.SetTop(bottomRect, Canvas.GetTop(topRect) + roiRect_1.Height-5);
            ((Rectangle)sender).CaptureMouse();

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
                //Canvas.SetTop(bottomRect, top + newHeight);
                //Debug.WriteLine(startPoint_1.Y.ToString()+" "+roiRect_1.Height.ToString() + "   "+ Canvas.GetTop(bottomRect).ToString() + "   " + currentPosition.Y.ToString()); ;


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

                Debug.WriteLine(startPoint_1.Y.ToString() + " " + roiRect_1.Height.ToString() + "   " + Canvas.GetTop(bottomRect).ToString() + "   " + currentPosition.Y.ToString()); ;
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



        private void image_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void image_1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void image_1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //isDragging = false;
            //if (selectedRect != null)
            //{
            //    selectedRect.ReleaseMouseCapture();
            //    selectedRect = null;
            //}
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
                Canvas.SetTop(bottomRect, marginTop + roiRect_1.Height - 5);


                Canvas.SetLeft(rightRect, marginLeft + roiRect_1.Width - 5);
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
            Debug.WriteLine(image_1.Width);
            Debug.WriteLine(image_1.Height);
            ImageSource imageSource = image_1.Source;
            string imagePath = "D://TestFolder//Image//cat//wallpaper.jpg";
            Uri imageUri = new Uri(System.IO.Path.GetFullPath(imagePath));

            BitmapImage bitmapImage = new BitmapImage(imageUri);
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            // 使用获取到的宽度和高度进行后续处理
            Debug.WriteLine("Image Width: " + width);
            Debug.WriteLine("Image Height: " + height);
            MessageBox.Show($"左上角X偏移：{Canvas.GetLeft(roiRect_1) * width / 300}\n" +
                $"左上角Y偏移：{Canvas.GetTop(roiRect_1) * height / 300}\n" +
                $"右下角X偏移：{(Canvas.GetLeft(roiRect_1) + roiRect_1.Width) * width / 300}\n" +
                $"右下角Y偏移：{(Canvas.GetTop(roiRect_1) + roiRect_1.Height) * height / 300}", "结果");
        }
    }
}
