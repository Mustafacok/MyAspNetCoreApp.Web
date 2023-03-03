using System.Net;

namespace MiddlewareExample.Web.Middlewares
{
    public class WhiteIpAdressControlMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string WhiteIpAdress = "::1";
        public WhiteIpAdressControlMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            //IPV4 => 127.0.01 => localhost
            //IPV6 => ::1 => localhost

            var reqIpAdress = context.Connection.RemoteIpAddress;
            bool anyWhiteIpAdress = IPAddress.Parse(WhiteIpAdress).Equals(reqIpAdress);

            if (anyWhiteIpAdress==true)
            {
                await _requestDelegate(context);
            }
            else
            {
                context.Response.StatusCode=HttpStatusCode.Forbidden.GetHashCode();
                await context.Response.WriteAsync("Forbidden");

            }
        }
    }
}
