using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using static HomeBankingMindHub.Services.ServiceExceptions;

namespace HomeBankingMindHub
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case MaxAccountsException ex:
                    context.Response.StatusCode = 403;
                    break;

                case ClientNotFoundException ex:
                    context.Response.StatusCode = 403;
                    break;

                default:
                    context.Response.StatusCode = 500;
                    break;
            }

            context.Response.ContentType = "text/plain";
            context.Response.WriteAsync(exception.Message);
        }
    }
}
