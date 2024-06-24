using StargateAPI.Business.Data;
using System.Net;

namespace StargateAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, StargateContext stargate)
        {

            var log = new RequestLog()
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                RequestReceivedDate = DateTime.Now,
                RequestBody = await ReadRequestBody(context.Request)
            };

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
                log.StatusCode = context.Response.StatusCode;
                log.ResponseBody = await ReadResponseBody(context.Response);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                log.ResponseBody = ex.Message;
                log.StatusCode = (int)HttpStatusCode.InternalServerError;
                await responseBody.CopyToAsync(originalBodyStream);
            }
            log.ResponseSentDate = DateTime.Now;

            await stargate.AddAsync(log);
            await stargate.SaveChangesAsync();
        }


        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }
    }
}
