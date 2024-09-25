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
        private MultithreadEventLoopGroup _group;
        private int _reconnectAttempts = 0; 
        private const int MaxReconnectAttempts = 2; 
        
        public event Action<string> OnError; 

        public ConnectionManager(IPAddress host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task ConnectAsync()
        {
            _group = new MultithreadEventLoopGroup();
            try
            {

                var bootstrap = new Bootstrap();
                bootstrap.Group(_group)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ClientInitializer());

                _channel = await bootstrap.ConnectAsync(_host, _port);
                Console.WriteLine("Connected to server.");
                _reconnectAttempts = 0;
                await Task.WhenAny(_channel.CloseCompletion);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Connection failed: {ex.Message}");
                await ReconnectAsync();
            }
            finally
            {
                await _group.ShutdownGracefullyAsync();
            }
        }

        public async Task ReconnectAsync()
        {
            while (_channel == null || !_channel.Active)
            {
                _reconnectAttempts++;
                if (_reconnectAttempts > MaxReconnectAttempts)
                {
                    string errorMessage = $"Failed to reconnect after {_reconnectAttempts} attempts.";
                    Console.WriteLine(errorMessage);
                    OnError?.Invoke(errorMessage); 
                }
                Console.WriteLine("Attempting to reconnect...");
                await Task.Delay(2000); // 等待 2 秒后重试
                await ConnectAsync();
            }
        }
    }
}
