using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace StargateAPI.Business.Models
{
    public class BaseResponse : IActionResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Successful";
        public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = ResponseCode;
            response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(this);
            await response.WriteAsync(json);
        }
    }
}