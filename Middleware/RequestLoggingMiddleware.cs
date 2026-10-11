namespace UserManagementApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            _logger.LogInformation("Incoming HTTP Request: {Method} {Path}", context.Request.Method, context.Request.Path);

            await _next(context);

            watch.Stop();
            _logger.LogInformation("Processed HTTP Response: {Method} {Path} | Status: {StatusCode} | Duration: {Elapsed}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, watch.ElapsedMilliseconds);
        }
    }
}