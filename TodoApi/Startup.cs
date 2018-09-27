using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.UseMvc();

            // app.Use(async (context, next) =>
            // {
            //     if (context.Request.Path == "/ws")
            //     {

            //         if (context.WebSockets.IsWebSocketRequest)
            //         {
            //             WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //             await Echo(context, webSocket);
            //         }
            //         else
            //         {
            //             context.Response.StatusCode = 400;
            //         }
            //     }
            //     else
            //     {
            //         await next();
            //     }

            // });
        }

        // private async Task Echo(HttpContext context, WebSocket webSocket)
        // {
        //     var buffer = new byte[1024 * 4];
        //     WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        //     ArraySegment<byte> encoded = Encoding.UTF8.GetBytes("{\"name\": \"John\"}");

        //     while (!result.CloseStatus.HasValue)
        //     {
        //         CancellationTokenSource cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(8));
        //         await this.RepeatActionEvery(() =>
        //         {
        //             webSocket.SendAsync(encoded, result.MessageType, result.EndOfMessage, CancellationToken.None);
        //         }, TimeSpan.FromSeconds(1), cancellation.Token);

        //         result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //     }
        //     await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        // }

        // private async Task RepeatActionEvery(Action action,
        //   TimeSpan interval, CancellationToken cancellationToken)
        // {
        //     while (true)
        //     {
        //         action();
        //         Task task = Task.Delay(interval, cancellationToken);

        //         try
        //         {
        //             await task;
        //         }
        //         catch (TaskCanceledException)
        //         {
        //             return;
        //         }
        //     }
        // }
    }
}