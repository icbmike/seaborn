using System.Collections.Specialized;

namespace seaborn.server
{
    public static class HttpRequestBuilder
    {
        public static HttpRequest Build(string requestText)
        {
            var firstLineBreakPosition = requestText.IndexOf("\r\n");
            var endOfHeadersPosition = requestText.IndexOf("\r\n\r\n");

            var requestLine = requestText.Substring(0, firstLineBreakPosition);
            var requestHeaders = requestText.Substring(firstLineBreakPosition, endOfHeadersPosition - firstLineBreakPosition);
            var body = requestText.Substring(endOfHeadersPosition);

            var requestLineParts = requestLine.Split(' ');

            return new HttpRequest
            {
                Method = ParseRequestMethod(requestLineParts[0]),
                Path = requestLineParts[1],
                Headers = ParseHeaders(requestHeaders),
                Body = body
            };
        }

        private static HttpRequestMethod ParseRequestMethod(string methodText)
        {
            return methodText switch
            {
                "GET" => HttpRequestMethod.Get,
                "POST" => HttpRequestMethod.Post,
                "PUT" => HttpRequestMethod.Put,
                "DELETE" => HttpRequestMethod.Delete,
                "HEAD" => HttpRequestMethod.Head,
                "OPTIONS" => HttpRequestMethod.Options
            };
        }

        private static NameValueCollection ParseHeaders(string requestHeaders)
        {
            return new NameValueCollection();
        }
    }
}
