using System.Linq;
using System.Text;

namespace seaborn.server
{
    internal class HttpResponseBuilder
    {
        internal static byte[] GetBytes(HttpResponse response)
        {
            var requestTextBuilder = new StringBuilder($"HTTP/1.1 {(int)response.Status} {GetStatusText(response.Status)}");

            var headers = response.Headers.AllKeys.Select(headerName => $"{headerName}: {response.Headers[headerName]}");
            requestTextBuilder.AppendJoin("\r\n", headers);

            requestTextBuilder.AppendLine();
            requestTextBuilder.AppendLine();

            requestTextBuilder.Append(response.Body);

            return Encoding.UTF8.GetBytes(requestTextBuilder.ToString());
        }

        private static string GetStatusText(HttpResponseStatus responseStatus) =>
            responseStatus switch
            {
                HttpResponseStatus.OK => "OK",
                HttpResponseStatus.BadRequest => "Bad Request",
                HttpResponseStatus.NotFound => "Not Found",
                _ => ""
            };
    }
}