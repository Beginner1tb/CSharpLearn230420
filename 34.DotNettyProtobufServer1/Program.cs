using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf;
using Prototest;  // 使用你的Protobuf命名空间
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;

namespace _34.DotNettyProtobufServer1
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            
            var otherBossGroup = new MultithreadEventLoopGroup(1);
            var otherWorkerGroup = new MultithreadEventLoopGroup();
            
            var udpWorkerGroup=new MultithreadEventLoopGroup();
            IPEndPoint serverIpEndPoint = new IPEndPoint(IPAddress.Loopback, 8081);
            try
            {
                
                
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ServerInitializer());

                IChannel boundChannel = bootstrap.BindAsync(8080).Result;
                Console.WriteLine("Server1 started.");
                //await boundChannel.CloseCompletion;
                
                var otherBootstrap = new ServerBootstrap();
                otherBootstrap.Group(otherBossGroup, otherWorkerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ServerInitializer());

                IChannel otherBoundChannel = bootstrap.BindAsync(9090).Result;
                Console.WriteLine("Server other started.");
                
                var udpBootstrap = new Bootstrap();  // UDP 不使用 ServerBootstrap
                udpBootstrap.Group(udpWorkerGroup)
                    .Channel<SocketDatagramChannel>()
                    .Handler(new UdpServerInitializer());
                var udpChannel=await udpBootstrap.BindAsync(serverIpEndPoint);
                Console.WriteLine("Udp Server started.");  
                
                // 等待TCP通道关闭和其他服务
                await Task.WhenAny(boundChannel.CloseCompletion, otherBoundChannel.CloseCompletion);

            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
                Task.WaitAny(udpWorkerGroup.ShutdownGracefullyAsync());
            }
        }
    }

    public class ServerHandler : SimpleChannelInboundHandler<MyMessage>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, MyMessage msg)
        {
            Console.WriteLine($"Received: Id = {msg.Id}, Content = {msg.Content}");
            // Echo the message back,回写msg数据信息
            ctx.WriteAndFlushAsync(msg);
        }
        
        #region 优先传入ChannelRead，如果有特定格式需求则不能启用
        // public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        // {
        //     Console.WriteLine("22222");
        //     if (msg is IByteBuffer)
        //     {
        //         var messgae = (IByteBuffer)msg;
        //         Console.WriteLine($"111111,Received {messgae.ToString(Encoding.UTF8)}");
        //     }
        //
        // }
        #endregion
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            // 客户端断开连接的处理
            Console.WriteLine("Client disconnected.");
            base.ChannelInactive(context);
        }
        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception exception)
        {
            Console.WriteLine("Exception: " + exception.ToString());
            ctx.CloseAsync();
        }
    }

    public class ServerInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            //这里规定了接收数据的格式
            pipeline.AddLast(new ProtobufVarint32FrameDecoder());
            pipeline.AddLast(new ProtobufDecoder(MyMessage.Parser));
            pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
            pipeline.AddLast(new ProtobufEncoder());
            pipeline.AddLast(new ServerHandler());
        }
    }
    
    public class UdpServerInitializer : ChannelInitializer<IChannel>
    {
        protected override void InitChannel(IChannel channel)
        {
            // 添加处理器，处理收到的 UDP 数据
            channel.Pipeline.AddLast(new UdpMessageHandler());
        }
    }

    public class UdpMessageHandler : SimpleChannelInboundHandler<DatagramPacket>
    {
        protected override void ChannelRead0(IChannelHandlerContext context, DatagramPacket packet)
        {
            // 获取接收到的字节数据
            var message = packet.Content.ToString(Encoding.UTF8);
            Console.WriteLine($"Received UDP message: {message} from {packet.Sender}");
        
            // 这里可以添加处理逻辑，比如回应客户端等
        }
        

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine($"Exception: {exception.Message}");
            context.CloseAsync();
        }
    }
}
