using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
namespace _27.ConcurrentTcpClient.ViewModels
{

    public class ClientViewModel : BindableBase
    {
        private ObservableCollection<string> _messages;
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
    
    public class MainWindowViewModel:BindableBase
    {
        private int _currentConnections;
        public int CurrentConnections
        {
            get { return _currentConnections; }
            set { SetProperty(ref _currentConnections, value); }
        }
        
        private int _totalConnections;
        public int TotalConnections
        {
            get { return _totalConnections; }
            set { SetProperty(ref _totalConnections, value); }
        }

        public int _current;
        public int _total;
        
        private ObservableCollection<ClientViewModel> _clients;
        public ObservableCollection<ClientViewModel> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }
        
        public MainWindowViewModel()
        {
            CurrentConnections = 0;
            TotalConnections = 0;
            _current = 0;
            _total = 0;
            Clients = new ObservableCollection<ClientViewModel>();
            StartServer();
        }

        private async void StartServer()
        {
            await StartAsync(IPAddress.Any, 9999);
        }
        
        public async Task StartAsync(IPAddress ipAddress,int port)
        {
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine("Server started");
            while (true)
            {
                TcpClient client=await listener.AcceptTcpClientAsync();
                
                Interlocked.Increment(ref _current);
                Interlocked.Increment(ref _total);
                CurrentConnections = _current;
                TotalConnections = _total;
                
                ClientViewModel clientViewModel = new ClientViewModel();
               
                Clients.Add(clientViewModel);
                
                HandleClientAsync(client,clientViewModel);
            }
        }
        
        private async void HandleClientAsync(TcpClient client,ClientViewModel clientViewModel)
        {
            clientViewModel.Messages=new ObservableCollection<string>();
            Console.WriteLine("Client connected");
            NetworkStream stream=client.GetStream();
            byte[] buffer=new byte[1024];
            int bytesRead;
            try
            {
                while ((bytesRead=await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");
                    
                    clientViewModel.AddMessage(message);
                    byte[] response = System.Text.Encoding.ASCII.GetBytes("Message Received");
                    await stream.WriteAsync(response, 0, response.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            finally
            {
               // client.Close();
                        
                Interlocked.Decrement(ref _current);
                CurrentConnections = _current;
                
                Clients.Remove(clientViewModel);
                Console.WriteLine("Client disconnected");
            }
        
        }
    }

}
// public class TcpServer
// {
//     public async Task StartAsync(IPAddress ipAddress,int port)
//     {
//         TcpListener listener = new TcpListener(ipAddress, port);
//         listener.Start();
//         Console.WriteLine("Server started");
//         while (true)
//         {
//             TcpClient client=await listener.AcceptTcpClientAsync();
//             HandleClientAsync(client);
//         }
//     }
//     private async void HandleClientAsync(TcpClient client)
//     {
//     
//         Console.WriteLine("Client connected");
//         NetworkStream stream=client.GetStream();
//         byte[] buffer=new byte[1024];
//         int bytesRead;
//         try
//         {
//             while ((bytesRead=await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
//             {
//                 string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                 Console.WriteLine($"Received: {message}");
//                 byte[] response = System.Text.Encoding.UTF8.GetBytes("Message Received");
//                 await stream.WriteAsync(response, 0, response.Length);
//             }
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//
//         }
//         finally
//         {
//             client.Close();
//         }
//         
//     }
//    
// }
