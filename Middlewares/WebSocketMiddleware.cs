using System.Net;
using System.Net.WebSockets;
using virtual_web_socket.Handlers;

namespace virtual_web_socket.Middlewares
{

    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.WebSockets.IsWebSocketRequest)
            {
                await _next(httpContext);
                return;
            }

            string channelName = (string)httpContext.GetRouteValue("channelName");

            if (string.IsNullOrEmpty(channelName))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await _next(httpContext);
                return;
            }

            var buffer = new byte[1024 * 4];

            WebSocket clientSocket = await httpContext.WebSockets.AcceptWebSocketAsync();

            await _webSocketHandler.OnConnected(clientSocket, channelName);

            while (clientSocket.State == WebSocketState.Open)
            {
                var result = await clientSocket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                    cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocketHandler.OnDisconnected(channelName);
                    return;
                }
            }
        }
    }
}
