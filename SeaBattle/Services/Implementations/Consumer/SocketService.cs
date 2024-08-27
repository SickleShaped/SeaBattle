
using SeaBattle.Models;
using Fleck;
using System.Net.WebSockets;
using System.Diagnostics.Contracts;

namespace SeaBattle.Services.Implementations.Consumer
{
    public class SocketService
    {
        public static Dictionary<string, WebSocket> Sockets = new Dictionary<string, WebSocket>();


        public static void SetSocket(string login, WebSocket webSocket, IConfiguration configuration)
        {
            if(!Sockets.TryGetValue(login, out WebSocket? socket))
            {
                Sockets.Add(login, webSocket);
                ConsumerService.ReadAllMessageFromClient(CancellationToken.None, login, configuration);
            }
            
        }

        public static WebSocket GetSocket(string login)
        {
            Sockets.TryGetValue(login, out WebSocket socket);
            return socket;
        }
    }
}
