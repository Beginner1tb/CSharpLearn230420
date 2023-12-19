using System;
using System.Collections.Generic;
using System.Drawing;
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
using _17.InterfaceChildView.IDepository;

namespace _17.InterfaceChildView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChildWindow childWindow;
        public MainWindow()
        {
            InitializeComponent();
            childWindow = new ChildWindow();
            childWindow.Show();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            childWindow.UpdateContent("111111");
            Bitmap bitmap = new Bitmap(@"C:\Users\fork2\Desktop\测试图片\cat\cat (1).bmp");
            BitmapImage bitmapImage = BitmapToImageSource(bitmap);
            childWindow.ShowImg(bitmapImage);
            bitmap.Dispose();
            
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
