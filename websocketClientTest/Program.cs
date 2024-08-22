using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace websocketClientTest
{
    class Program
    {

        static ulong _sending = 0;
        static async Task Main(string[] args)
        {

            using (ClientWebSocket client = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://localhost:8080/ws?clientID=asd&client=gfg"); // 서버 주소 설정
                CancellationTokenSource cts = new CancellationTokenSource();

                await client.ConnectAsync(serverUri, cts.Token);

                Task.Run(() => ReceiveMessage(client, cts)); // 메시지 수신 시작

                while (client.State == WebSocketState.Open)
                {
                    //동기화 함수 이전 값이 1일경우 if문을 통과하고 다음 로직실행.
                    //데이터 전송 후 서버로 부터 데이터를 받을때까지 대기
                    if (Interlocked.Read(ref _sending) == 1 )
                        continue;

                    Console.Write("메시지 입력: ");
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cts.Token);

                    Interlocked.CompareExchange(ref _sending, 1, 0);
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
                Console.WriteLine($"from server massage : {message}");


                Interlocked.CompareExchange(ref _sending, 0, 1);
            }
        }
    }
}