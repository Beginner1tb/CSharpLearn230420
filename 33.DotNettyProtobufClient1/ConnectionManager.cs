using DotNetty.Transport.Bootstrapping;
using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Threading;

namespace _33.DotNettyProtobufClient1
{
    public class ConnectionManager
    {
        internal IChannel Channel;

     
        internal MultithreadEventLoopGroup Group;
        private readonly IPAddress _host;
        private readonly int _port;
       
        private int _reconnectAttempts = 0; 
        private const int MaxReconnectAttempts = 2;
        // 用于确保不会多次触发重连的标志位
        private bool _isReconnecting = false;
        public bool IsConnected = false;

        public event Action<string> OnError; 

        public ConnectionManager(IPAddress host, int port)
        {
            _host = host;
            _port = port;
            Group = new MultithreadEventLoopGroup(4);

        }

        public async Task ConnectAsync()
        {
           
            try
            {
                //if (_group != null)
                //{
                //    await _group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
                //}
                if (Channel != null && Channel.Active)
                {
                    // 关闭旧连接，释放资源
                    await Channel.CloseAsync();
                    Channel = null;
                }

                
                var bootstrap = new Bootstrap();
                bootstrap.Group(Group)
                    .Channel<TcpSocketChannel>()
                    .Handler(new ClientInitializer(this));

                Channel = await bootstrap.ConnectAsync(_host, _port);
                Console.WriteLine("Connected to server.");
                _reconnectAttempts = 0;
                IsConnected = true;
                await Task.WhenAny(Channel.CloseCompletion);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Connection failed: {ex.Message}");
                IsConnected = false;
                await ReconnectAsync();
               
            }

            //finally
            //{
            //    await _group.ShutdownGracefullyAsync();

            //}
        }

        public async Task ReconnectAsync()
        {
            // 如果已经在重连中，直接返回
            if (_isReconnecting)
            {
                return;
            }

            _isReconnecting = true; // 开始重连
            try
            {
                while (Channel == null || !Channel.Active)
                {
                    _reconnectAttempts++;
                    if (_reconnectAttempts > MaxReconnectAttempts)
                    {
                        string errorMessage = $"Failed to reconnect after {_reconnectAttempts} attempts.";
                        Console.WriteLine(errorMessage);
                        OnError?.Invoke(errorMessage);
                    }
                    Console.WriteLine("Attempting to reconnect...");
                    IsConnected = false;
                    await Task.Delay(2000); // 等待 2 秒后重试
                    await ConnectAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               // throw;
            }
            finally
            {
                _isReconnecting = false; // 重连完成，解除锁定
            }

        }
        
        public async Task ShutdownAsync()
        {
            if (Channel != null && Channel.Active)
            {
                await Channel.CloseAsync(); // 关闭连接
            }

            if (Group != null)
            {
                await Group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)); // 优雅关闭事件循环
                Group = null;
            }

            
        }
    }
}
