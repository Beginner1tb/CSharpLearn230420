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


namespace _32.TCPprotobufServer1
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageData receivedImageData = ImageReceiver.ReceiveImageData(5000);

            // string outputBmpPath = "output.bmp";
            //
            // ProtobufImageHandler.RebuildBmpFromImageData(receivedImageData, outputBmpPath);
        }
    }

    public static class ImageReceiver
    {
        public static ImageData ReceiveImageData(int port)
        {
            // 开启TCP监听
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("服务器启动，等待连接...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                listener.Start();
                Console.WriteLine("客户端已连接！");
                Task.Run(() =>
                {
                    if (client != null) HandleClient(client);
                });
            }

            // using (TcpClient client = listener.AcceptTcpClient())
            // using (NetworkStream stream = client.GetStream())
            // {
            //     // 接收数据大小
            //     byte[] dataSize = new byte[4];
            //     stream.Read(dataSize, 0, dataSize.Length);
            //     int imageSize = BitConverter.ToInt32(dataSize, 0);
            //
            //     // 接收图片数据
            //     byte[] receivedData = new byte[imageSize];
            //     int totalBytesRead = 0;
            //     while (totalBytesRead < imageSize)
            //     {
            //         int bytesRead = stream.Read(receivedData, totalBytesRead, imageSize - totalBytesRead);
            //         totalBytesRead += bytesRead;
            //     }
            //
            //     // 反序列化为ImageData
            //     //BinaryFormatter formatter = new BinaryFormatter();
            //     //using (MemoryStream memoryStream = new MemoryStream(receivedData))
            //     //{
            //     //    ImageData imageData = (ImageData)formatter.Deserialize(memoryStream);
            //     //    return imageData;
            //     //}
            //
            //     ImageData imageData = ImageData.Parser.ParseFrom(receivedData);
            //     return imageData;
            // }
        }

        private static void HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    // 接收数据长度
                    byte[] dataLengthBuffer = new byte[4];
                    stream.Read(dataLengthBuffer, 0, dataLengthBuffer.Length);
                    int dataLength = BitConverter.ToInt32(dataLengthBuffer, 0);

                    // 接收图片数据
                    byte[] receivedData = new byte[dataLength];
                    int totalBytesRead = 0;
                    while (totalBytesRead < dataLength)
                    {
                        int bytesRead = stream.Read(receivedData, totalBytesRead, dataLength - totalBytesRead);
                        totalBytesRead += bytesRead;
                    }

                    // 反序列化为 ImageData 对象
                    ImageData imageData = ImageData.Parser.ParseFrom(receivedData);

                    // 生成一个唯一的文件名
                    string outputImagePath = $"received_image_{DateTime.Now.ToString("yyyyMMddssfff")}.bmp";

                    // 重构 BMP 图片并保存
                    ProtobufImageHandler.RebuildBmpFromImageData(imageData, outputImagePath);

                    Console.WriteLine($"图片数据接收并保存成功：{outputImagePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"客户端处理错误: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }

    public static class ProtobufImageHandler
    {
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