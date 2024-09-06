using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace _30.ConsoleProtobufTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            string bmpFilePath = "input.bmp";
            string serializedFilePath = "image_data.bin";

            // 加载 BMP 图片并生成 ImageData
            ImageData imageData = ProtobufImageHandler.LoadBmpAndSerialize(bmpFilePath);

            // 序列化 ImageData 到文件
            ProtobufImageHandler.SerializeImageDataToFile(imageData, serializedFilePath);

            Console.WriteLine("图片序列化成功！");

            string outputBmpPath = "output.bmp";

            // 从文件反序列化 ImageData
            ImageData deserializedImageData = ProtobufImageHandler.DeserializeImageDataFromFile(serializedFilePath);

            // 重建 BMP 图片并保存
            ProtobufImageHandler.RebuildBmpFromImageData(deserializedImageData, outputBmpPath);

            Console.WriteLine("图片反序列化并重构成功！");

            Console.ReadLine();
        }
    }

    public static class ProtobufImageHandler
    {
        public static ImageData LoadBmpAndSerialize(string imagePath)
        {
            using (Bitmap bmp = new Bitmap(imagePath))
            {
                // 获取图片的宽度、高度和像素格式
                int width = bmp.Width;
                int height = bmp.Height;
                PixelFormat format = bmp.PixelFormat;

                // 锁定位图的像素数据
                BitmapData bmpData = bmp.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly,
                    format);

                // 计算像素数组的大小
                int bytesPerPixel = Image.GetPixelFormatSize(format) / 8;
                int byteCount = bmpData.Stride * height;

                // 创建一维数组来存储像素数据
                byte[] pixelArray = new byte[byteCount];
                Marshal.Copy(bmpData.Scan0, pixelArray, 0, byteCount);

                // 解锁位图
                bmp.UnlockBits(bmpData);

                // 创建并返回ImageData实例
                ImageData imageData = new ImageData
                {
                    Width = width,
                    Height = height,
                    Format = format.ToString(),  // 保存格式信息为字符串
                    PixelArray = ByteString.CopyFrom(pixelArray)  // 将字节数组存储到Protobuf的bytes类型
                };

                return imageData;
            }
        }

        public static void SerializeImageDataToFile(ImageData imageData, string filePath)
        {
            // 序列化 ImageData 到文件
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                imageData.WriteTo(fs); // 使用 Protobuf 序列化
            }
        }


        public static ImageData DeserializeImageDataFromFile(string filePath)
        {
            // 从文件反序列化 ImageData
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return ImageData.Parser.ParseFrom(fs); // 使用 Protobuf 反序列化
            }
        }

        public static void RebuildBmpFromImageData(ImageData imageData, string outputPath)
        {
            int width = imageData.Width;
            int height = imageData.Height;

            // 创建新的位图对象，使用 24 位的 RGB 格式
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                BitmapData bmpData = bmp.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);

                // 将ImageData中的像素数据复制回位图内存中
                Marshal.Copy(imageData.PixelArray.ToByteArray(), 0, bmpData.Scan0, imageData.PixelArray.Length);

                // 解锁位图
                bmp.UnlockBits(bmpData);

                // 保存为BMP格式
                bmp.Save(outputPath, ImageFormat.Bmp);
            }
        }
    }

}
