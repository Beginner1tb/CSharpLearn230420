using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf;
using Prototest;  // 使用你的Protobuf命名空间
using System;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;

namespace _34.DotNettyProtobufServer1
{
    class Program
    {
        static void Main(string[] args)
        {
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ServerInitializer());

                IChannel boundChannel = bootstrap.BindAsync(8080).Result;
                Console.WriteLine("Server started.");
                boundChannel.CloseCompletion.Wait();
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
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

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {

            if (msg is IByteBuffer)
            {
                var messgae = (IByteBuffer)msg;
                Console.WriteLine($"111111,Received {messgae.ToString(Encoding.UTF8)}");
            }

        }

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
}
