using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
//using HalconDotNet;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using HalconDotNet;
using System.Drawing;
using System.Drawing.Imaging;

namespace _6.Halcon_TestForNuget
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
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
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\Users\\LFLFLF\\Desktop\\";
                openFileDialog.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tiff;";
                openFileDialog.RestoreDirectory = true;


                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

                    Img1.Source = bitmapImage;

                    int imageTest_Height = 0;

                    ///从文件读取到内存流中
                    Stream imageStream = new MemoryStream();
                    byte[] imageData = File.ReadAllBytes(filepath);
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
                        imageTest_Height = hv_img_h.I;
                        ho_Monkey.Dispose();
                        ho_Region.Dispose();

                        hv_Height.Dispose();
                        hv_Width.Dispose();
                        hv_Ratio.Dispose();
                        hv_img_h.Dispose();


                    }

                    System.Console.WriteLine(imageTest_Height);
                }
            }
        }


        private Out1 Algorithm_Test1(byte[] imageData)
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
                return out1;

                //imageTest_Height = hv_img_h.I;
                ho_Monkey.Dispose();
                ho_Region.Dispose();

                hv_Height.Dispose();
                hv_Width.Dispose();
                hv_Ratio.Dispose();
                hv_img_h.Dispose();


            }
        }



        private void Btn_AlgoEncap_Click(object sender, RoutedEventArgs e)
        {
            string filepath = string.Empty;
           // string filecontent = string.Empty;
            BitmapImage bitmapImage = new BitmapImage();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\Users\\LFLFLF\\Desktop\\";
                openFileDialog.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tiff;";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

                    Img1.Source = bitmapImage;

                    int imageTest_Height = 0;

                    ///从文件读取到内存流中
                    Stream imageStream = new MemoryStream();
                    byte[] imageData = File.ReadAllBytes(filepath);

                    Out1 out1 = new Out1();
                    Algorithem algorithem = new Algorithem();

                    out1 = algorithem.Algorithm_Test1(imageData);
                    Console.WriteLine("封装算法测试" + out1.height1.ToString()); ;
                   
                   // System.Windows.MessageBox.Show($"ImageHeight is {out1.height1.ToString()}\nImageWidth is {out1.width1.ToString()}");
                }

            }
        }
    }
}
