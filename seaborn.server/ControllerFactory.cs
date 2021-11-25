using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace seaborn.server
{
    public class ControllerRouter : IRouter
    {
        private readonly Dictionary<string, Type> _controllerMappings;

        public ControllerRouter()
        {
            _controllerMappings = Assembly.GetExecutingAssembly().GetTypes()
                .Where(typeof(Controller).IsAssignableFrom)
                .ToDictionary(type => type.Name.Substring(0, type.Name.IndexOf("Controller")).ToLower());
        }

        private static Func<HttpRequest, HttpResponse> NotFound = (_) => new HttpResponse
        {
            Status = HttpResponseStatus.NotFound,
            Body = "Not Found"
        };

        public Func<HttpRequest, HttpResponse> RouteRequest(HttpRequest request)
        {
            var pathParts = request.Path.Split("/");

            var controllerType = _controllerMappings.ContainsKey(pathParts[1])
                ? _controllerMappings[pathParts[1]]
                : null;            

            if (controllerType == null)
                return NotFound;

            var actionName = pathParts.Length > 2
                ? pathParts[2]
                : "Index";

            var controllerMethodInfo = controllerType.GetMethods()
                .SingleOrDefault(
                    methodInfo => methodInfo.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase) &&
                    methodInfo.IsPublic &&
                    methodInfo.ReturnType == typeof(HttpResponse)
                );

            if(controllerMethodInfo == null)
                return NotFound;

            var controller = Activator.CreateInstance(controllerType);
            var @delegate = controllerMethodInfo.CreateDelegate(typeof(Func<HttpRequest, HttpResponse>), controller);

            return (Func<HttpRequest, HttpResponse>)@delegate;
        }
    }
}