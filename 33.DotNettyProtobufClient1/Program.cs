using DotNetty.Codecs.Protobuf;
using DotNetty.Transport;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf;
using Prototest;  // 使用你的Protobuf命名空间
using System;
using System.Net;
using DotNetty.Transport.Bootstrapping;
using System.Threading.Tasks;
using System.Timers;

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
        public override async void ChannelActive(IChannelHandlerContext ctx)
        {
            try
            {
                var message = new MyMessage { Id = 1, Content = "Hello, DotNetty Server!" };
                await ctx.WriteAndFlushAsync(message);
                Console.WriteLine("ChannelActive : "+DateTime.Now.Millisecond);
                await Task.Delay(5).ContinueWith(t => ctx.CloseAsync());
                // 发送数据并等待完成
                // await  ctx.WriteAndFlushAsync(message).ContinueWith(task =>
                // {
                //     if (task.Status == TaskStatus.RanToCompletion)
                //     {
                //         ctx.CloseAsync();
                //     }
                //     else
                //     {
                //         Console.WriteLine("Failed to send message: " + task.Exception?.Message);
                //         ctx.CloseAsync(); // 即便发送失败，也关闭连接
                //     }
                // });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ChannelActive: {ex.Message}");
            }
           
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, MyMessage msg)
        {
            Console.WriteLine($"Received echo: Id = {msg.Id}, Content = {msg.Content}");
            //await ctx.CloseAsync();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);  
            // 客户端断开连接的处理
            Console.WriteLine("IChannelHandlerContext : "+DateTime.Now.Millisecond);
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
