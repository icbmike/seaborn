namespace seaborn.server.Controllers
{
    public class HelloController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return new HttpResponse
            {
                Status = HttpResponseStatus.OK,
                Headers = new System.Collections.Specialized.NameValueCollection(),
                Body = "Hello"
            };
        }

        public HttpResponse Hello(HttpRequest request)
        {
            return new HttpResponse
            {
                Status = HttpResponseStatus.OK,
                Headers = new System.Collections.Specialized.NameValueCollection(),
                Body = "Hello hello"
            };
        }
    }
}
