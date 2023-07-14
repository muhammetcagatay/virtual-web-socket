namespace virtual_web_socket.Dtos
{
    public class SocketSendMessageRequestDto
    {
        public string ChannelName { get; set; } = string.Empty;
        public int MessageCount { get; set; } = 1;
        public int Timeout { get; set; } = 100; // Millisecond

    }
}
