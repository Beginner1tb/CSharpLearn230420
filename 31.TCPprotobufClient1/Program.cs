using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace _31.TCPprotobufClient1
{
    class Program
    {
        static void Main(string[] args)
        {
            string bmpFilePath = "input.bmp";

            // 加载 BMP 图片并生成 ImageData
            ImageData imageData = ProtobufImageHandler.LoadBmpAndSerialize(bmpFilePath);

            ImageSender.SendImageData(imageData, "127.0.0.1", 5000);
        }
    }

    public static class ImageSender
    {
        public static void SendImageData(ImageData imageData, string serverIp, int port)
        {
            using (TcpClient client = new TcpClient(serverIp, port))
            using (NetworkStream stream = client.GetStream())
            {
                // 序列化ImageData为字节数组
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    imageData.WriteTo(memoryStream); // 使用 Protobuf 序列化
                    byte[] dataToSend = memoryStream.ToArray();

                    // 发送数据大小
                    byte[] dataSize = BitConverter.GetBytes(dataToSend.Length);
                    // stream.Write(dataSize, 0, dataSize.Length);

                    // 发送图片数据
                    // stream.Write(dataToSend, 0, dataToSend.Length);

                    // 创建一个包含长度信息和数据的字节数组
                    byte[] combinedData = new byte[dataSize.Length + dataToSend.Length];

                    // 将 dataLength（长度信息）复制到 combinedData 的前 4 字节
                    Buffer.BlockCopy(dataSize, 0, combinedData, 0, dataSize.Length);

                    // 将 dataToSend（实际数据）复制到 combinedData 的后续部分
                    Buffer.BlockCopy(dataToSend, 0, combinedData, dataSize.Length, dataToSend.Length);

                    // 一次性发送合并后的字节数组
                    stream.Write(combinedData, 0, combinedData.Length);
                }
            }
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
                    //注意这里的类型是ByteString的不可变数组，原因见Google Protocol Buffers
                };

                return imageData;
            }
        }


    }
}
