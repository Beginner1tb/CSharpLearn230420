using System;
using System.IO;
using Google.Protobuf;
using ProtobufExample;
using ProtobufImage;

namespace _29.protobufTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageTest();
            Console.ReadLine();
        }

        static void PersonTest1()
        {
            Person person = new Person
            {
                Id = 1,
                Name = "张三",
                Email = "zhangsan@example.com"
            };

            // 序列化 Person 对象到文件
            using (var output = File.Create("person.jpg"))
            {
                person.WriteTo(output);
                Console.WriteLine("Person 对象已序列化到 person.bin 文件。");
            }

            // 从文件反序列化 Person 对象
            Person deserializedPerson;
            using (var input = File.OpenRead("person.jpg"))
            {
                deserializedPerson = Person.Parser.ParseFrom(input);
                Console.WriteLine("从 person.bin 文件反序列化得到的 Person 对象：");
                Console.WriteLine($"ID: {deserializedPerson.Id}");
                Console.WriteLine($"Name: {deserializedPerson.Name}");
                Console.WriteLine($"Email: {deserializedPerson.Email}");
                
            }
        }

        static void ImageTest()
        {
            string imagePath = "./color.bmp";
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // 创建ImageMessage并设置图片数据
            var imageMessage = new ImageMessage
            {
                ImageData = ByteString.CopyFrom(imageBytes)
            };

            // 序列化ImageMessage
            byte[] serializedData;
            using (var memoryStream = new MemoryStream())
            {
                imageMessage.WriteTo(memoryStream);
                serializedData = memoryStream.ToArray();
            }

            // 反序列化ImageMessage
            ImageMessage deserializedImageMessage;
            using (var memoryStream = new MemoryStream(serializedData))
            {
                deserializedImageMessage = ImageMessage.Parser.ParseFrom(memoryStream);
            }

            // 将字节数组保存为图片文件
            string outputImagePath = "./color1111.bmp";
            File.WriteAllBytes(outputImagePath, deserializedImageMessage.ImageData.ToByteArray());

            Console.WriteLine("图片传递完成并保存到 " + outputImagePath);
        }
    }
}
