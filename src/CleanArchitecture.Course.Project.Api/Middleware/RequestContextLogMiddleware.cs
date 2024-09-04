using Serilog.Context;

namespace CleanArchitecture.Course.Project.Api.Middleware
{
    public class RequestContextLogMiddleware(RequestDelegate next)
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";

        private readonly RequestDelegate _next = next;

        private static string GetCorrelationId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);

            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }

        public Task Invoke(HttpContext context)
        {
            using(LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
            {
                return _next(context);
            }
        }
    }
}