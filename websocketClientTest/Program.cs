using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace websocketClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (ClientWebSocket client = new ClientWebSocket())
            {
                //Uri serverUri = new Uri("ws://localhost:8080/ws"); // 서버 주소 설정
                Uri serverUri = new Uri("ws://localhost:8080/ws?clientID=asd&client=gfg"); // 서버 주소 설정
                CancellationTokenSource cts = new CancellationTokenSource();
                // HTTP 헤더에 클라이언트 ID 추가
                //client.Options.SetRequestHeader("X-Client-Id", "your_client_id"); // 실제 ID로 변경

                await client.ConnectAsync(serverUri, cts.Token);

                Task.Run(() => ReceiveMessage(client, cts)); // 메시지 수신 시작

                while (client.State == WebSocketState.Open)
                {
                    Console.Write("메시지 입력: ");
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);
                }

                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token);
            }
        }

        static async Task ReceiveMessage(ClientWebSocket client, CancellationTokenSource cts)
        {
            byte[] buffer = new byte[1024];
            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                if (result.MessageType == WebSocketMessageType.Close) break;
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"서버 메시지: {message}");
            }
        }
    }
}