using System;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace websocketserverTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new HttpListener();
            server.Prefixes.Add("http://localhost:8080/"); // 원하는 포트 설정
            server.Start();
            Console.WriteLine("WebSocket 서버 시작... (http://localhost:8080/) ");

            while (true)
            {//
                var context = await server.GetContextAsync();

                // CORS 설정 (모든 Origin 허용)
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                if (context.Request.IsWebSocketRequest && context.Request.Url.AbsolutePath == "/ws")
                {
                    // HttpListenerWebSocketContext 객체를 얻고 AcceptWebSocketAsync() 호출
                    var webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                    WebSocket webSocket = webSocketContext.WebSocket;


                    var query = context.Request.Url.Query;
                    var clientId = System.Web.HttpUtility.ParseQueryString(query).Get("clientId");

                    if (string.IsNullOrEmpty(clientId))
                    {
                        await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Client ID is required."));
                        context.Response.Close();
                        continue;
                    }


                    // HTTP 헤더에서 클라이언트 ID 가져오기 (webSocketContext 사용)
                    //string clientId = webSocketContext.Headers["X-Client-Id"];
                    //if (string.IsNullOrEmpty(clientId))
                    //{
                    //    await webSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, "Client ID is required.", CancellationToken.None);
                    //    continue;
                    //}



                    Task.Run(() => HandleWebSocket(webSocket));
                }
                else
                {
                    // 다른 요청 처리 (예: HTTP 응답)
                    context.Response.StatusCode = 404; // WebSocket 경로가 아닌 경우
                    context.Response.Close();
                }
            }
        }

        static async Task HandleWebSocket(WebSocket webSocket)
        {
            var buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"클라이언트 메시지: {message}");

                    // 에코 기능 (클라이언트에게 메시지 다시 전송)
                    var response = Encoding.UTF8.GetBytes($"{message}");
                    byte[] asd = new byte[] { 1, 2, 3 };

                    Debug.WriteLine($"[서버로그메시지] 전송전 :{message} ");
                    await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                    Debug.WriteLine($"[서버로그메시지] 전송완료 :{message} ");

                }
            }
        }
    }
}