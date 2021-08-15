using System.Collections.Specialized;

namespace seaborn.server
{
    public class HttpResponse
    {
        public HttpResponseStatus Status { get; set; }
        public NameValueCollection Headers { get; set; }
        public string Body { get; set; }
    }

    public enum HttpResponseStatus
    {
        OK = 200,
        BadRequest = 400,
        NotFound = 404
    }
}