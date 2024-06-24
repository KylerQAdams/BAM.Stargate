using Microsoft.AspNetCore.Mvc.Filters;
using StargateAPI.Business.Models;
using System.Net;

namespace StargateAPI.Middleware
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.Result = new BaseResponse()
            {
                Message = exception.Message,
                Success = false,
                ResponseCode = context.Exception switch
                {
                    BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                }
            };

            context.ExceptionHandled = true;
        }
    }
}
