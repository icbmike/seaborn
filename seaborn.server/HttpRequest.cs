using System.Collections.Specialized;

namespace seaborn.server
{
    public class HttpRequest
    {
        public HttpRequestMethod Method { get; set; }
        public string Path { get; set; }
        public NameValueCollection Headers { get; set; }
        public string Body { get; set; }
    }
}
