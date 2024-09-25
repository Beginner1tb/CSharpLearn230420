using DotNetty.Transport.Bootstrapping;
using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;

namespace _33.DotNettyProtobufClient1
{
    public class ConnectionManager
    {
        private IChannel _channel;
        private readonly IPAddress _host;
        private readonly int _port;
        

        public ConnectionManager(IPAddress host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task ConnectAsync()
        {
            var _group = new MultithreadEventLoopGroup();
            try
            {
                
                var bootstrap = new Bootstrap();
                bootstrap.Group(_group)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ClientInitializer());

                _channel = await bootstrap.ConnectAsync(_host, _port);
                Console.WriteLine("Connected to server.");
                await Task.WhenAny(_channel.CloseCompletion);
            }
            catch (Exception ex)
            {
                await _group.ShutdownGracefullyAsync();
                Console.WriteLine($"Connection failed: {ex.Message}");
                await ReconnectAsync();
            }
        }

        public async Task ReconnectAsync()
        {
            while (_channel == null || !_channel.Active)
            {
                Console.WriteLine("Attempting to reconnect...");
                await Task.Delay(2000); // 等待 2 秒后重试
                await ConnectAsync();
            }
        }
    }
}
