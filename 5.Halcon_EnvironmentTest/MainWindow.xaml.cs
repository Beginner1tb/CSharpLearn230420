using System.Windows;
//using HalconDotNet;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using HalconDotNet;
using System.Drawing;
using System.Drawing.Imaging;

namespace _5.Halcon_EnvironmentTest
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
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var filecontent = reader.ReadToEnd();
                    }

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


    }
}
