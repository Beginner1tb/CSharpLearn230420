### 1. Protobuf的.proto文件使用方法
#### 1. 建立``xxx.proto``文件
形式如下
```csharp
syntax = "proto3";//使用协议
package com.example.project;//命名空间是com.example.project，可不写,默认当前命名空间
message ImageData //相当于Class
{
int32 width = 1;//1,2,3,4是序号
int32 height = 2;
string format = 3;
bytes pixel_array = 4;
}
```
注意，这里的 package com.example.project指定了命名空间，ImageData 就属于该命名空间下，在 C# 中，它会映射为 Com.Example.Project.ImageData
通过这种方式，你可以在不同的 .proto 文件中使用相同的消息名，而不会造成冲突，因为它们被限定在不同的命名空间下。

示例如下，两个width消息，不冲突
```csharp
com.example.project1.width
com.example.project2.width  
```
#### 2. 利用protoc.exe将``xxx.proto``转换成``xxx.cs``文件
简单示例的代码如下，使用CLI，在当前文件夹下生成
```text
protoc --csharp_out=. .\xxx.proto
```
注意：

第一，要将protoc.exe加入到环境变量的PATH中，才能使用命令行protoc

第二，CMD下，``.\xxx.proto``路径可以不用加引号，但是在PowerShell中，由于``.\xxx.proto``中间有符号，需要加双引号，否则不识别路径

#### 3.将``xxx.cs``加入到所需要的工程中
类名``ImageData``可以正常使用，如果``package com.example.project``类似的命名空间，那么需要添加``using com.example.project``

### 2.Protobuf使用方法(有点类似JSON)
#### 1. 客户端(发送端)——序列化
A. 将信息按``xxx.cs``中的``ImageData``类形式进行赋值
B. 将实例化``ImageData``的对象以二进制形式存入某个文件中，后缀名不限，如下
```csharp
// 序列化 ImageData 到文件
string filePath = "image_data.bin";
using (FileStream fs = new FileStream(filePath, FileMode.Create))
{
    imageData.WriteTo(fs); // 使用 Protobuf 序列化
}
```
也可以利用TCP发送出去，注意，发送的时候需要加入数据头，表示图片属于固定长度，原因在C
```csharp
SendImageData(imageData, "127.0.0.1", 5000);//SendImageData是发送TCP的方法
```
C. Protobuf中，byte[] 类型在 .proto 文件中对应的是 bytes 类型

在 C# 中，Protocol Buffers 的 bytes 类型被映射为 ByteString，而不是直接映射为 byte[]，这是因为 ByteString 是一个不可变的字节序列，旨在提高效率和安全性。

所以在bytes相关的类时，需要通过``ByteString.CopyFrom(byte[])``将byte[]转成ByteString
#### 2. 服务端(接收端)——反序列化
A. 核心代码如下
```csharp
ImageData imageData = ImageData.Parser.ParseFrom(receivedData);//receivedData可以是文件也可以是TCP的buffer，存储形式是btye[]二进制数据流
```
B. TCP接受的时候，注意需要获取数据长度的数据头，由此获取固定长度的memorystream buffer

C. 接收到数据``imageData``后就可以按``ImageData``的格式进行操作了

