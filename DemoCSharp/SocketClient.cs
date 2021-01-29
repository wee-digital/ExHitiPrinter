using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DemoCSharp
{
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class SocketClient
    {

        ClientWebSocket socketClient = new ClientWebSocket();

        WebSocketReceiveResult result;

        public interface IListener
        {
            void OnStateChanged(WebSocketState state);

            void OnMessage(string text);

            void OnMessage(MemoryStream ms);


            void OnError(Exception e);
        }

        public IListener listener;


        public SocketClient(IListener listener)
        {
            this.listener = listener;
        }

        public async void Connect(string url)
        {
            try
            {
                socketClient = new ClientWebSocket();
                await socketClient.ConnectAsync(new Uri(url), CancellationToken.None);
                listener.OnStateChanged(socketClient.State);
                Receive();
            }
            catch (Exception e)
            {
                listener.OnError(e);
            }
        }

        public async void Close()
        {
            try
            {
                await socketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                listener.OnStateChanged(socketClient.State);
            }
            catch (Exception e)
            {
                listener.OnError(e);
            }
        }

        public void Send(string message)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                socketClient.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                listener.OnError(e);
            }

        }

        public async void Receive()
        {
            while (socketClient.State == WebSocketState.Open)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {

                        do
                        {
                            var receiveBuffer = ClientWebSocket.CreateClientBuffer(1024, 1024);
                            result = await socketClient.ReceiveAsync(receiveBuffer, CancellationToken.None);
                            ms.Write(receiveBuffer.Array, 0, result.Count);
                        } while (!result.EndOfMessage);

                        switch (result.MessageType) {
                            case WebSocketMessageType.Text:
                                var text = Encoding.UTF8.GetString(ms.ToArray());
                                listener.OnMessage(text);
                                break;
                            case WebSocketMessageType.Binary:
                                listener.OnMessage(ms);
                                break;
                        }

                    
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;
                    }

                }
                catch (Exception e)
                {
                    listener.OnError(e);
                }
            }


        }

        public bool IsOpen()
        {
            return socketClient.State == WebSocketState.Open;
        }


    }

}
