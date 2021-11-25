using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace seaborn.server
{
    class Program
    {
        const int BufferSize = 2048;

        static void Main(string[] args)
        {
            var router = new ControllerRouter();

            var listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            var port = 8005;
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(5);

            Console.WriteLine($"[Info] [{DateTime.UtcNow:O}] Server Started - listening on port {port}");

            while (true)
            {
                var connection = listener.Accept();

                var next = new Random().Next(1000, 1000000);
                var requestId = Convert.ToString(next, 16);

                Task.Run(() => HandleConnection(connection, router, requestId));
            }
        }

        private static void HandleConnection(Socket connection, IRouter controllerFactory, string reqId)
        {
            var dataBuffer = new byte[BufferSize];

            string request = "";
            int bytesRecieved;

            do
            {
                bytesRecieved = connection.Receive(dataBuffer);
                request += Encoding.ASCII.GetString(dataBuffer, 0, bytesRecieved);
            }
            while (bytesRecieved == BufferSize);

            var httpRequest = HttpRequestBuilder.Build(request);

            Console.WriteLine($"[Info] [{DateTime.UtcNow:O}] Handling request [{reqId}] - {httpRequest.Method.ToString().ToUpper()} {httpRequest.Path}");

            var requestHandler = controllerFactory.RouteRequest(httpRequest);
            var response = requestHandler.Invoke(httpRequest);

            var responseBytes = HttpResponseBuilder.GetBytes(response);

            connection.Send(responseBytes);
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
        }
    }
}
