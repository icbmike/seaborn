using System;

namespace seaborn.server
{
    public interface IRouter
    {
        Func<HttpRequest, HttpResponse> RouteRequest(HttpRequest request);
    }
}