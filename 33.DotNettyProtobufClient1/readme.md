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