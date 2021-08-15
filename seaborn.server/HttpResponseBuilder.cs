using System.Text;

namespace seaborn.server
{
    internal class HttpResponseBuilder
    {
        internal static byte[] GetBytes(HttpResponse response)
        {
            return Encoding.ASCII.GetBytes(response.Body);
        }
    }
}