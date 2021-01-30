using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using WebApplication.Socket;
using System.Text;
using System.IO;

namespace WebApplication
{
    public class Startup
    {

        WebSocket WebSocketAndroid;
        WebSocket WebSocketWin;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureFile(app, env);
            ConfigureWebSocketAndroid(app, env);
            ConfigureWebSocketWin(app, env);
        }

        public void ConfigureFile(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }



        public void ConfigureWebSocketAndroid(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/android")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocketAndroid = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await EchoAndroid();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
        }

        private async Task EchoAndroid()
        {
            while (WebSocketWin == null || WebSocketWin.State != WebSocketState.Open)
            {
            }
            try
            {
                var buffer = new byte[1024 * 16];
                WebSocketReceiveResult result = await WebSocketAndroid.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                System.Console.WriteLine("receive a message from Android");
                while (!result.CloseStatus.HasValue)
                {
                    await WebSocketWin.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    result = await WebSocketAndroid.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await WebSocketAndroid.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

            }
            catch (Exception e) 
            {
                System.Console.WriteLine(e.Message);
            }
        }


        public void ConfigureWebSocketWin(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/win")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocketWin = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await EchoWin();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
           

        }

        private async Task EchoWin()
        {
            while (WebSocketAndroid == null || WebSocketAndroid.State != WebSocketState.Open)
            {
            }
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await WebSocketWin.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await WebSocketWin.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await WebSocketWin.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                System.Console.WriteLine("receive a message");
            }
            await WebSocketWin.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

    }
}
