using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{

    [Route("ws")]
    [ApiController]
    public class WsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if (this.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    // var buffer = new byte[1024 * 4];
                    // WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // ArraySegment<byte> encoded = Encoding.UTF8.GetBytes("{\"name\": \"John\"}");

                    // while (!result.CloseStatus.HasValue)
                    // {
                    //     CancellationTokenSource cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                    //     await this.RepeatActionEvery(() =>
                    //     {
                    //         webSocket.SendAsync(encoded, result.MessageType, result.EndOfMessage, CancellationToken.None);
                    //     }, TimeSpan.FromSeconds(1), cancellation.Token);

                    //     result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    // }
                    // await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                    while (true)
                    {
                        var response = string.Format("Hello! Time {0}", System.DateTime.Now.ToString());
                        var bytes = System.Text.Encoding.UTF8.GetBytes(response);

                        await webSocket.SendAsync(new System.ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);

                        await Task.Delay(2000);
                    }
                }
            }

            return null;
        }

        private async Task RepeatActionEvery(Action action,
         TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                action();
                Task task = Task.Delay(interval, cancellationToken);

                try
                {
                    await task;
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}