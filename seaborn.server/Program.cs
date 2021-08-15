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

            listener.Bind(new IPEndPoint(IPAddress.Any, 8000));
            listener.Listen(5);

            while (true)
            {
                var connection = listener.Accept();

                Task.Run(() => HandleConnection(connection, router));
            }
        }

        private static void HandleConnection(Socket connection, IRouter controllerFactory)
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

            var requestHandler = controllerFactory.RouteRequest(httpRequest);
            var response = requestHandler.Invoke(httpRequest);

            var responseBytes = HttpResponseBuilder.GetBytes(response);

            connection.Send(responseBytes);
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
        }
    }
}
