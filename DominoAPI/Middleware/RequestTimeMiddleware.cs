using System.Diagnostics;

namespace DominoAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();

            await next.Invoke(context);

            _stopwatch.Stop();
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds > 4500)
            {
                var message =
                    $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds}";

                _logger.LogInformation(message);
            }
        }
    }
}