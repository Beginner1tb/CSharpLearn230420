using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Drawing;
using Image = System.Drawing.Image;
using System.Runtime.InteropServices;

namespace _7.Queue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        ConcurrentQueue<Image> imgQueue ;

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        public MainWindow()
        {
            InitializeComponent();
            // QueueTest();
        }

        private void QueueTest()
        {
            ConcurrentQueue<string> myQueue = new ConcurrentQueue<string>();
            myQueue.Enqueue("1");
            myQueue.Enqueue("2");
            myQueue.Enqueue("3");

            //存在
            if (myQueue.Contains<string>("1"))
            {
                Debug.WriteLine("yes");
            }
            //移除指定元素
            string element_1 = "1";
            if (myQueue.TryDequeue(out element_1))
            {
                Debug.WriteLine("Success");
            }
            List<string> list_MyQueue = myQueue.ToList();
            ObservableCollection<string> list = new ObservableCollection<string>(list_MyQueue); //myQueue.ToList();
            foreach (var queMem in list_MyQueue)
            {
                Debug.WriteLine(queMem);
            }

            Debug.WriteLine("--------------");
            foreach (var queMem in list)
            {
                Debug.WriteLine(queMem);
            }



        }

        /// 操作必须保持异步
        private async void Btn_ImgShow_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = @"D:\TestFolder\Image\cat";
            string[] filePaths = await GetFilePathsAsync(folderPath);
            int count = 0;
            ConcurrentQueue<Image> myQueue = new ConcurrentQueue<Image>();

            imgQueue = new ConcurrentQueue<Image>();
            lock (imgQueue)
            {
                while (!imgQueue.IsEmpty)
                {
                    imgQueue.TryDequeue(out _);
                }

            }
            foreach (var item in filePaths)
            {
                Debug.WriteLine(item);
                using (FileStream fileStream = new FileStream(item, FileMode.Open, FileAccess.Read))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        Image image = Image.FromStream(memoryStream);
                        // myQueue.Enqueue(image);
                        imgQueue.Enqueue(image);
                        count++;
                        Debug.WriteLine("操作数" + count);

                        //using (MemoryStream memoryStream1 = new MemoryStream())
                        //{
                        //    image.Save(memoryStream1, System.Drawing.Imaging.ImageFormat.Bmp);
                        //    memoryStream1.Position = 0;
                        //    BitmapImage bitmapImage = new BitmapImage();
                        //    bitmapImage.BeginInit();
                        //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        //    bitmapImage.StreamSource = memoryStream1;
                        //    bitmapImage.EndInit();
                        //    Img1.Source = bitmapImage;
                        //}

                        //await Task.Delay(100);
                    }
                }

            }

            //await Task.Delay(1000);

            //lock (myQueue)
            //{
            //    while (!myQueue.IsEmpty)
            //    {
            //        myQueue.TryDequeue(out _);
            //    }

            //}

            ////myQueue = null;
            //GC.Collect();
        }

        public async Task<string[]> GetFilePathsAsync(string folderPath)
        {
            return await Task.Run(() => Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories));
        }

        private void Btn_ImgShow_Deque_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image image;
            
            if (imgQueue == null)
            {
                MessageBox.Show("");
            }
            else
            {
                if (!imgQueue.IsEmpty)
                {
                    bool b = imgQueue.TryPeek(out image);
                    if (b)
                    {
                        //Image image1 = new Bitmap(1, 1);
                        //// image1.Dispose();
                        //image1 = image;
                        using (Bitmap bitmap= new Bitmap(image))
                        {

                            IntPtr hBitmap = bitmap.GetHbitmap();

                            try
                            {
                                BitmapSource bitmapSource = 
                                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    hBitmap, 
                                    IntPtr.Zero, Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());

                                Img1.Source = bitmapSource;
                            }
                            finally
                            {
                                DeleteObject(hBitmap);
                            }

            //                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //bitmap.GetHbitmap(),
            //IntPtr.Zero,
            //System.Windows.Int32Rect.Empty,
            //BitmapSizeOptions.FromEmptyOptions());

                          //  Img1.Source = bitmapSource;
                           
                          //  DeleteObject(bitmap.GetHbitmap());

                            //using (MemoryStream memoryStream1 = new MemoryStream())
                            //{
                            //    image1.Save(memoryStream1, System.Drawing.Imaging.ImageFormat.Bmp);
                            //    memoryStream1.Position = 0;
                            //    BitmapImage bitmapImage = new BitmapImage();
                            //    bitmapImage.BeginInit();
                            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            //    bitmapImage.StreamSource = memoryStream1;
                            //    bitmapImage.EndInit();
                            //    Img1.Source = bitmapImage;
                            //}

                            imgQueue.TryDequeue(out image);
                        }
                 

                    }

                }
                else
                {
                    Img1.Source = null;
                    imgQueue = null;
                    GC.Collect();
                }
            }




        }
    }
}
