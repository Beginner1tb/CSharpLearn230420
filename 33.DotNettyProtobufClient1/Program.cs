using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf;
using Prototest;  // 使用你的Protobuf命名空间
using System;
using System.Net;
using DotNetty.Transport.Bootstrapping;

namespace _33.DotNettyProtobufClient1
{
    class Program
    {
        static void Main(string[] args)
        {
            var workerGroup = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap.Group(workerGroup)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ClientInitializer());

                IChannel clientChannel = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080)).Result;
                clientChannel.CloseCompletion.Wait();
            }
            finally
            {
                workerGroup.ShutdownGracefullyAsync().Wait();
            }
        }
    }

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
}
