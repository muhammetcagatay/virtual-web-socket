using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace virtual_web_socket.Handlers
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public void AddSocket(WebSocket socket, string channel)
        {
            _sockets.TryAdd(channel, socket);
        }

        public async Task RemoveSocket(string channelName)
        {
            WebSocket socket;
            _sockets.TryRemove(channelName, out socket);

            if (socket != null)
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the ConnectionManager",
                                        cancellationToken: CancellationToken.None);
            }

        }
    }
}
