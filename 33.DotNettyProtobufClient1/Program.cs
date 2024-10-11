using DotNetty.Codecs.Protobuf;
using DotNetty.Transport;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf;
using Prototest;  // 使用你的Protobuf命名空间
using System;
using System.Net;
using System.Net.Sockets;
using DotNetty.Transport.Bootstrapping;
using System.Threading.Tasks;
using System.Timers;

namespace _33.DotNettyProtobufClient1
{
    class Program
    {

        static async Task Main(string[] args)
        {

            // var workerGroup = new MultithreadEventLoopGroup();
            // try
            // {
            //     var bootstrap = new Bootstrap();
            //     bootstrap.Group(workerGroup)
            //         .Channel<TcpSocketChannel>()
            //         .Handler(new ClientInitializer());
            //
            //     IChannel clientChannel = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080)).Result;
            //     
            //     //clientChannel.CloseCompletion.Wait();
            //     //更优雅的方式，阻塞当前的tcp进程，等待结束
            //     await Task.WhenAny(clientChannel.CloseCompletion);
            // }
            // finally
            // {
            //     workerGroup.ShutdownGracefullyAsync().Wait();
            // }

            // 订阅错误事件

            var manager = new ConnectionManager(IPAddress.Parse("127.0.0.1"), 8080);

            manager.OnError += (errorMessage) =>
            {
                Console.WriteLine($"Error: {errorMessage}");
                    // 可以在这里处理其他逻辑
                };

            await manager.ConnectAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();


        }
    }

    public class ClientHandler : SimpleChannelInboundHandler<MyMessage>
    {
        private readonly ConnectionManager _connectionManager;

        public ClientHandler(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public override async void ChannelActive(IChannelHandlerContext ctx)
        {
            try
            {
                var message1 = new MyMessage { Id = 1, Content = "Hello, DotNetty Server!1" };

                await ctx.WriteAndFlushAsync(message1);
                await Task.Delay(1000);
                var message2 = new MyMessage { Id = 2, Content = "Hello, DotNetty Server!2" };

                await ctx.WriteAndFlushAsync(message2);
                await Task.Delay(1000);
                var message3 = new MyMessage { Id = 3, Content = "Hello, DotNetty Server!3" };

                await ctx.WriteAndFlushAsync(message3);
                await Task.Delay(1000);

                //这里时间跟电脑和网络有关，不一定能保证读写通道完全关闭
                //await Task.Delay(100).ContinueWith(t => ctx.CloseAsync());
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

        public override async void ChannelInactive(IChannelHandlerContext ctx)
        {
            //base.ChannelInactive(ctx);  
            // 客户端断开连接的处理，不做处理
            Console.WriteLine("IChannelHandlerContext : " + DateTime.Now.Millisecond);
            //关不关看上去区别不大
            //ctx.CloseAsync();


            //这里是错误写法，通过ctx这个ChannelHandler获取ConnectionManager
            //但是此时，ConnectionManager现在已关闭，无法重连
            //((ConnectionManager)ctx.Channel.Parent).ReconnectAsync();
            try
            {
                await _connectionManager.ReconnectAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("ReconnectAsync Failed:" + e);

            }
        }

        public override async void ExceptionCaught(IChannelHandlerContext ctx, Exception exception)
        {
            
            Console.WriteLine("ExceptionCaught: " + exception.ToString());
            //Console.ReadLine();
            //await ctx.CloseAsync();
            //if (exception is SocketException)
            //{
            //    // 捕获远程主机强制关闭连接的异常，不退出，等待 ChannelInactive 处理
            //    Console.WriteLine("Remote host forcibly closed the connection.");
            //    ctx.FireChannelInactive(); // 让 ChannelInactive 处理断开和重连
            //}
            //else
            //{
            //    //ctx.CloseAsync(); // 对其他异常类型，关闭连接
            //}
        }

    }

    public class ClientInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly ConnectionManager _connectionManager;

        public ClientInitializer(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        protected override void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new ProtobufVarint32FrameDecoder());
            pipeline.AddLast(new ProtobufDecoder(MyMessage.Parser));

            pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
            pipeline.AddLast(new ProtobufEncoder());
            pipeline.AddLast(new ClientHandler(_connectionManager));

        }
    }
}
