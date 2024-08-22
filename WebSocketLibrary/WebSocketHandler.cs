using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketLibrary
{
    public class WebSocketHandler
    {
        private WebSocket _webSocket;
        private byte[] _buffer = new byte[1024];

        public event EventHandler<string> MessageReceived;

        public WebSocketHandler(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (_webSocket.State == WebSocketState.Open)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(_buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(_buffer, 0, result.Count);
                        OnMessageReceived(message);

                        // 예시: 받은 메시지를 그대로 클라이언트에게 에코
                        await SendAsync($"Echo from server: {message}", cancellationToken);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
                    }
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
            finally
            {
                Cleanup();
            }
        }

        public async Task SendAsync(string message, CancellationToken cancellationToken)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        private void Cleanup()
        {
            try
            {
                _webSocket.Dispose();
            }
            catch { }
        }
    }
}
