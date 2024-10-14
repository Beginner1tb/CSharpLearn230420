using System;
using Xunit;
using Moq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using _33.DotNettyProtobufClient1;
using Xunit.Abstractions;
using IChannel = DotNetty.Transport.Channels.IChannel;

namespace _35.DotNettyProtobufClient1.Test
{
    public class Tests
    {
        //Moq 不能用于模拟或重写非虚方法,所该段有误
        //[Fact]
        //public async Task ConnectAsync_Should_EstablishConnectionSuccessfully()
        //{

        //    // 安排
        //    var mockChannel = new Mock<IChannel>();
        //    mockChannel.Setup(c => c.Active).Returns(true);

        //    var connectionManager = new Mock<ConnectionManager>(IPAddress.Parse("127.0.0.1"), 8080)
        //    {
        //        CallBase = true
        //    };
        //    connectionManager.Setup(m => m.ConnectAsync()).Returns(Task.FromResult(mockChannel.Object));

        //    // 执行
        //    await connectionManager.Object.ConnectAsync();

        //    // 断言
        //    Assert.True(mockChannel.Object.Active, "ConnectAsync 方法调用后连接应处于激活状态");
        //}

        //private readonly ITestOutputHelper _output;

        //public Tests(ITestOutputHelper output)
        //{
        //    _output = output;
        //}

        [Fact]
        public async Task ConnectAsync_Should_EstablishConnectionSuccessfully()
        {
            // 安排
            var mockChannel = new Mock<IChannel>();
            mockChannel.Setup(c => c.Active).Returns(true);

            // 使用实际的 ConnectionManager 实例而不是模拟对象
            var connectionManager = new ConnectionManager(IPAddress.Parse("127.0.0.1"), 8080);

            // 创建取消令牌以防止连接无限等待
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000); // 设置 2 秒后取消

            try
            {
                // 执行
                await Task.WhenAny(connectionManager.ConnectAsync(), Task.Delay(2000, cancellationTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
                // 捕获取消异常，以确保测试可以正常结束
            }

            // 断言
            Assert.True(connectionManager.IsConnected); // 确保对象已正确创建
        }

        //[Fact]
        //public async Task ConnectAsync_Should_EstablishConnectionSuccessfully_NotMoq()
        //{
        //    // 安排
        //    var connectionManager = new ConnectionManager(IPAddress.Parse("127.0.0.1"), 8080);

        //    // 执行
        //    await connectionManager.ConnectAsync();

        //    // 断言
        //    Assert.NotNull(connectionManager); // 确保对象已正确创建
        //}

        [Fact]
        public async Task ShutdownAsync_ShouldCloseChannelAndShutdownGroupGracefully()
        {
            // Arrange
            var mockChannel = new Mock<IChannel>();
            var mockGroup = new Mock<MultithreadEventLoopGroup>(4);

            // 模拟 _channel 和 _group
            mockChannel.Setup(channel => channel.Active).Returns(true);
            mockChannel.Setup(channel => channel.CloseAsync()).Returns(Task.CompletedTask);

            mockGroup.Setup(group => group.ShutdownGracefullyAsync(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            var connectionManager = new ConnectionManager(IPAddress.Loopback, 8080)
            {
                // 使用 mock 代替真实对象
                 Channel = mockChannel.Object,
                Group = mockGroup.Object
            };

            // Act
            await connectionManager.ShutdownAsync();

            // Assert
            // 确保 CloseAsync 被调用了一次
            mockChannel.Verify(channel => channel.CloseAsync(), Times.Once);

            // 确保 ShutdownGracefullyAsync 被调用了一次
            mockGroup.Verify(group => group.ShutdownGracefullyAsync(
                It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task ShutdownAsync_ShouldGracefullyShutdownEventLoopGroupAndCloseChannel()
        {
            // Arrange
            var group = new MultithreadEventLoopGroup(1);
            var connectionManager = new ConnectionManager(IPAddress.Loopback, 8080)
            {
                Group = group
            };

            // Act
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000); // 设置 2 秒后取消

            try
            {
                // 执行
                await Task.WhenAny(connectionManager.ConnectAsync(), Task.Delay(2000, cancellationTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
                // 捕获取消异常，以确保测试可以正常结束
            }


            await connectionManager.ShutdownAsync(); // 调用关闭方法

            // Assert
            // 验证事件循环组是否已优雅关闭
            Assert.True(group.IsShutdown); // 确认 group 被优雅关闭
            Assert.True(group.IsTerminated); // 确认 group 被终止

            // 验证通道是否被关闭
            var channel = connectionManager.Channel;
            if (channel != null)
            {
                Assert.False(channel.Active); // 通道应该已经被关闭
            }
        }

        private TcpListener _server;

        [Fact]
        public async Task ShutdownAsync_ShouldGracefullyShutdownEventLoopGroupAndCloseChannel_AfterSuccessfulConnection()
        {
            // 启动一个本地 TCP 服务器
            StartServer();

            // Arrange
            var group = new MultithreadEventLoopGroup(1);
            var connectionManager = new ConnectionManager(IPAddress.Loopback, 8080)
            {
                Group = group
            };

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000); // 设置 2 秒后取消

            try
            {
                // 执行
                await Task.WhenAny(connectionManager.ConnectAsync(), Task.Delay(2000, cancellationTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
                // 捕获取消异常，以确保测试可以正常结束
            }

            // 确保连接是否成功
            var channel = connectionManager.Channel;
            Assert.NotNull(channel);
            Assert.True(channel.Active, "连接应该是活动状态");

            await connectionManager.ShutdownAsync(); // 调用关闭方法

            // Assert
            // 验证事件循环组是否已优雅关闭
            Assert.True(group.IsShutdown, "事件循环组应该已经优雅关闭");
            Assert.True(group.IsTerminated, "事件循环组应该已经终止");

            // 验证通道是否被关闭
            Assert.False(channel.Active, "通道应该已经被关闭");
           

            StopServer(); // 停止服务器
        }

        private void StartServer()
        {
            _server = new TcpListener(IPAddress.Loopback, 8080);
            _server.Start();
            _server.BeginAcceptTcpClient(AcceptClientCallback, _server);
        }

        private void AcceptClientCallback(IAsyncResult ar)
        {
            try
            {
                var listener = (TcpListener)ar.AsyncState;
                var client = listener.EndAcceptTcpClient(ar);

                // 在后台线程中保持客户端连接，直到服务器停止
                Task.Run(() =>
                {
                    using (var stream = client.GetStream())
                    {
                        var buffer = new byte[1024];
                        while (client.Connected)
                        {
                            // 模拟处理客户端请求
                            if (stream.Read(buffer, 0, buffer.Length) == 0)
                                break;
                        }
                    }
                });
            }
            catch (ObjectDisposedException)
            {
                // 服务器被关闭时会触发该异常，不需要特殊处理
            }
        }

        private void StopServer()
        {
            _server.Stop();
        }
    }
}
