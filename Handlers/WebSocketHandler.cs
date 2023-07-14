using System.Net.WebSockets;
using System.Text;
using virtual_web_socket.Dtos;

namespace virtual_web_socket.Handlers
{
    public class WebSocketHandler
    {
        protected ConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketHandler(ConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket, string channelName)
        {
            WebSocketConnectionManager.AddSocket(socket, channelName);
        }

        public virtual async Task OnDisconnected(string channelName)
        {
            await WebSocketConnectionManager.RemoveSocket(channelName);
        }

        public async Task<string> SendMessageAsync(SocketSendMessageRequestDto messageSettings, string message)
        {
            WebSocket socket = WebSocketConnectionManager.GetSocketById(messageSettings.ChannelName);

            if (socket == null || socket.State != WebSocketState.Open)
                return "Socket not found or socket is closed";

            for (int i = 0; i < messageSettings.MessageCount; i++)
            {
                await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                        offset: 0,
                                                        count: message.Length),
                        messageType: WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: CancellationToken.None);

                Thread.Sleep(messageSettings.Timeout);
            }

            return $"{message} message sent to {messageSettings.ChannelName} channel";
        }

    }
}
