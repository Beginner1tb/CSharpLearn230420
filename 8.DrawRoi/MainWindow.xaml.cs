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


        private void roiRect_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(roiRect_1);
            isDragging = true;
            selectedRect = roiRect_1;
            roiRect_1.CaptureMouse();
        }

        private void topRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint_1 = e.GetPosition(topRect);
            isDragging = true;
            selectedRect = topRect;
            Canvas.SetTop(bottomRect, Canvas.GetTop(topRect) + roiRect_1.Height);
            topRect.CaptureMouse();
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

        private void roiRect_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == roiRect_1)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                double left = currentPosition.X - startPoint_1.X;
                double top = currentPosition.Y - startPoint_1.Y;

                Canvas.SetLeft(roiRect_1, left);
                Canvas.SetTop(roiRect_1, top);
            }
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
                //Canvas.SetTop(bottomRect, top + newHeight);
                Debug.WriteLine(startPoint_1.Y.ToString()+" "+roiRect_1.Height.ToString() + "   "+ Canvas.GetTop(bottomRect).ToString() + "   " + currentPosition.Y.ToString()); ;


                //double deltaY = startPoint.Y - currentPosition.Y;

                //double newTop = Canvas.GetTop(roiRect_1) - deltaY;
                ////double newHeight = initialHeight + deltaY;

                //Canvas.SetTop(roiRect, newTop);

            }
        }

        private void bottomRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == bottomRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                double bottom = currentPosition.Y - startPoint_1.Y;

                if (roiRect_1.Height - bottom >= 0)
                {
                    roiRect_1.Height -= bottom;
                }
                else
                {
                    roiRect_1.Height = 0;
                }

            }
        }

        private void leftRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == leftRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                double left = currentPosition.X - startPoint_1.X;

                Canvas.SetLeft(roiRect_1, left);
                roiRect_1.Width -= left;
            }
        }

        private void rightRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedRect == rightRect)
            {
                Point currentPosition = e.GetPosition(canvas_1);

                double right = currentPosition.X - startPoint_1.X;

                roiRect_1.Width += right;
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
            isDragging = false;
            if (selectedRect != null)
            {
                selectedRect.ReleaseMouseCapture();
                selectedRect = null;
            }
        }

        private void topRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            topRect.ReleaseMouseCapture();
        }
    }
}
