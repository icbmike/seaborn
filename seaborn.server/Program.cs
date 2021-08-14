using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace seaborn.server
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            listener.Bind(new IPEndPoint(IPAddress.Any, 8000));
            listener.Listen(5);

            const int BufferSize = 2048;
            var dataBuffer = new byte[BufferSize];

            while (true)
            {
                var connection = listener.Accept();

                string request = "";
                int bytesRecieved;

                do
                {
                    bytesRecieved = connection.Receive(dataBuffer);
                    request += Encoding.ASCII.GetString(dataBuffer, 0, bytesRecieved);
                }
                while (bytesRecieved == BufferSize);

                Console.WriteLine("Request:");
                Console.WriteLine(request);

                connection.Send(Encoding.ASCII.GetBytes("yea boi"));
                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }      
        }
    }
}
