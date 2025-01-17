### 一. DotNetty使用（客户端）
#### 1. Bootstrap 配置步骤
(1) 配置事件循环组``EventLoopGroup``；

(2) 配置通道类型``Channel``
包含TCP，UDP，Epoll，HTTP之类

如果使用的是TCP，注意客户端用的是``TcpSocketChannel ``，服务端是``TcpServerSocketChannel``

(3) 配置通道处理器

(4) 启动连接，注意客户端和服务端不同
#### 2. ``EventLoopGroup``是什么
(1) 代表线程循环组

(2) ``MultithreadEventLoopGroup``代表多线程处理的事件循环组，当然也能通过``MultithreadEventLoopGroup(n)``调整线程数

#### 3，Group代表什么含义
代表基础设置类``Bootstrap``配置几个线程事件循环组，一般客户端只要一个处理数据的事件循环组，服务端一般需要两个事件循环组，分别用于负责接受连接和处理数据读写
#### 4. 配置通道处理器（Handler）
(1) 这里通过 ``Handler`` 方法配置客户端通道的处理器，这个处理器通常是一个继承自 ``ChannelInitializer<T>`` 的类，它负责在客户端通道的管道中添加各种处理器，比如编码器、解码器、以及自定义的消息处理逻辑。

(2) 以上的类中，需要重载``InitChannel(T channel)``方法，用来声明一个管道(pipeline)，并在管道中添加各种各样的处理器

#### 5. 处理器的使用(以客户端为例)
以下为客户端的通道中处理器使用流程
```csharp
public class ClientInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new ProtobufVarint32FrameDecoder());
            pipeline.AddLast(new ProtobufDecoder(MyMessage.Parser));
            pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
            pipeline.AddLast(new ProtobufEncoder());
            pipeline.AddLast(new ClientHandler());
        }
    }
```

在DotNetty框架中，这些代码段分别设置了处理Protobuf消息的几个关键编解码器。每个处理器的作用如下：

1. ``ProtobufVarint32FrameDecoder``
   接收数据时：首先，ProtobufVarint32FrameDecoder处理TCP粘包问题，确保数据被正确分帧。
2. ``ProtobufDecoder``
   接着，ProtobufDecoder将分帧后的数据转换为Protobuf消息对象。
3. ``ProtobufVarint32LengthFieldPrepender``
   发送数据时：ProtobufVarint32LengthFieldPrepender添加消息长度前缀
4. ``ProtobufEncoder``
   随后ProtobufEncoder将Protobuf对象序列化为字节流。
5. ``ClientHandler``
   代表数据处理的核心过程

注意：``ProtobufVarint32FrameDecoder``和``ProtobufDecoder``是入站处理器，``ClientHandler``实际上也是入站处理器,``ProtobufEncoder``和``ProtobufVarint32LengthFieldPrepender``是出站处理器

入站处理器的顺序 需要从低级别到高级别，通常是 解帧 -> 解码 -> 业务处理。

出站处理器的顺序 需要从高级别到低级别，通常是 业务处理 -> 编码 -> 帧封装。

如果顺序颠倒，会导致处理逻辑失败，因为处理器期望的输入格式无法得到保证。

注意：以上是理论情况，但是目前位置还是按上述代码执行，后期再做修改，比如本代码中的代码顺序是
```csharp
pipeline.AddLast(new ProtobufVarint32FrameDecoder());
pipeline.AddLast(new ProtobufDecoder(MyMessage.Parser));
pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
pipeline.AddLast(new ProtobufEncoder());
pipeline.AddLast(new ClientHandler());
```
``ClientHandler``理论上是写数据帧，应该是出站业务处理的内容，但是此处只能放在最后，如果放在出站编码之前，则会无法发送

#### 6. 数据处理的核心流程
如上所示，``ClientHandler``是pipline中处理数据的核心流程，在此程序中其继承了``SimpleChannelInboundHandler<T>``类，``T``在这里代表的是protobuf的数据格式，这里是``MyMessage``，其完整代码如下：
````csharp
public class ClientHandler : SimpleChannelInboundHandler<MyMessage>
    {
        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            var message = new MyMessage { Id = 1, Content = "Hello, DotNetty Server!" };
            ctx.WriteAndFlushAsync(message);
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, MyMessage msg)
        {
            Console.WriteLine($"Received echo: Id = {msg.Id}, Content = {msg.Content}");
            await ctx.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception exception)
        {
            Console.WriteLine("Exception: " + exception.ToString());
            ctx.CloseAsync();
        }
    }
````
其中，
1. ``ChannelActive``方法代表该通道被激活时的处理过程，注意使用的是DotNetty的方法，比如发送数据就是``ctx.WriteAndFlushAsync(message);``
2. ``ChannelRead0``方法代表获取到``T``数据时的处理过程，如果是``ChannelRead``方法则不再使用``ChannelRead0``
3. ``ExceptionCaught``方法代表出现异常时的处理过程

#### 7. 关闭连接的方式
如果客户端发送完数据需要关闭连接，则需要在发送完成之后``await ctx.WriteAndFlushAsync(object message);``，相应的增加一个延迟，如果直接调用``ctx.CloseAsync()``，则会直接断开连接，会可能出现以下两种情况：

1，客户端强行中断，客户端运行Task报错，2，服务端还在读写数据的时候中断导致服务端报错

当然，也可以让服务端发送一个数据，然后关闭连接在``ChannelActive``方法中进行

注意：延迟关闭时间跟电脑和网络有关，不一定能保证读写通道完全关闭

#### 8. 断线重连
断线重连的程序运行逻辑如下：（因为PlantUML的图在Github中无法显示所以用mermaid图表）
```mermaid
---
title: 程序运行逻辑
---
flowchart TD
   A([Start]) --> A1[Init ConnectManager]
   A1[Init ConnectManager]-->B[ConnectAsync]
   B --> C{connected}
   C -- No --> D[ReconnectedAsync]
   
   C -- Yes --> E[ChannelActive]
   E --> F{ExceptionCaught}
   F -- Yes --> G[ChannelInActive]
   G --> D
   
   D[ReconnectedAsync]-->D2{_isReconnecting}
   D2 -- No --> D3[Channel State]
   D3 --> D4[Wait For Seconds]
   D4 -->B[ConnectAsync]
     
   

```
具体过程：
1. 初始化``ConnectManager``，``ConnectManager``作为管理整个链接的实际对象，输入参数为IP和端口，``ConnectManager``的构造函数的入口参数，注意，初始化``ConnectManager``时也初始化了``EventLoopGroup``,有且只有一次，注意这里一定需要加阻塞过程，这里用的是``Console.ReadLine()``，否则如果重连失败主线程会直接退出
2. 启动``ConnectManager``的``ConnectAsync``的Task，``ConnectAsync``中包含了一般的网络引导对象，以及引导的连接；
3. 此时如果连接失败，则进入重连的``ReconnectedAsync``的Task，如果连接成功，则等待链接结束Task:``Task.WhenAny(_channel.CloseCompletion);``，如果网络异常结束，则进入重连的``ReconnectedAsync``的Task；
4. 如果刚开始连接成功，后面网络中断，``ExceptionCaught``方法会捕捉到错误，``ChannelInActive``在之后处理断线之后的问题，在``ChannelInActive``中启用``ReconnectedAsync``的Task；
5. 当进入重连的``ReconnectedAsync``的Task时，先判断``_isReconnecting``是否处于重连状态，如果正在重连则退出，防止反复重连，如果不在重连状态中，先判断Channel的状态，如果Channel除非非活动状态或者没启动,则再运行``ConnectAsync``的Task；
6. 至此完成断线重连的所有逻辑

注意：
1. ``ConnectManager``初始化之后一定需要加阻塞过程，这里用的是``Console.ReadLine()``，因为如果不加阻塞，则如果一开始就没连上还会重连，如果一开始连上了再连就会主线程直接退出，总之，这个断线重连肯定是阻塞的；
2. ``EventLoopGroup``对象只初始化一次，防止每次重连的时候出现资源浪费；
3. 网络断线事件是在``ChannelInActive``中处理，``IChannelHandlerContext``在这个时间上看起来关不关闭对程序影响不大；
4. ``ConnectAsync``不能在失败后关闭``EventLoopGroup``，因为``EventLoopGroup``只初始化了一次，看样子``EventLoopGroup``只能在主线程关闭时再关闭