using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StargateAPI.Business.Data;
using StargateAPI.Middleware;
using StargateTests.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace StargateTests.UnitTests.Middleware
{
    [TestClass()]
    public class RequestLoggingMiddlewareTests : DataTestBase
    {
        private RequestLoggingMiddleware _middleware;

        [TestInitialize]
        public void Setup()
        {
            _middleware = new RequestLoggingMiddleware(next: (innerHttpContext) => Task.CompletedTask);
        }

        [TestMethod]
        public async Task Invoke_ShouldLogRequestAndResponse_Success()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "GET";
            httpContext.Request.Path = "/api/test";
            httpContext.Request.QueryString = new QueryString("?param=value");
            httpContext.Request.Body = new MemoryStream();
            await httpContext.Request.Body.WriteAsync(Encoding.UTF8.GetBytes("Request body"));
            var originalBodyStream = httpContext.Response.Body;
            var responseBody = new MemoryStream();
            httpContext.Response.Body = responseBody;



            // Act
            await _middleware.Invoke(httpContext, _dbContext);

            // Assert
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseContent = new StreamReader(responseBody).ReadToEnd();

            var loggedRequest = await _dbContext.RequestLogs.FindAsync(1); 

            Assert.IsNotNull(loggedRequest);
            Assert.AreEqual("GET", loggedRequest.Method);
            Assert.AreEqual("/api/test", loggedRequest.Path);
            Assert.AreEqual("?param=value", loggedRequest.QueryString);
            Assert.IsNotNull(loggedRequest.RequestBody);
            Assert.AreEqual(StatusCodes.Status200OK, loggedRequest.StatusCode);
            Assert.AreEqual(responseContent, loggedRequest.ResponseBody);
        }

        [TestMethod]
        public async Task Invoke_ShouldLogExceptionOnInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = "GET";
            httpContext.Request.Path = "/api/error";
            httpContext.Request.QueryString = new QueryString("?param=value");
            httpContext.Request.Body = new MemoryStream();
            await httpContext.Request.Body.WriteAsync(Encoding.UTF8.GetBytes("Request body"));

            var originalBodyStream = httpContext.Response.Body;
            var responseBody = new MemoryStream();


            _middleware = new RequestLoggingMiddleware(next: (innerHttpContext) => throw new Exception("Simulated error"));


            // Act
            try
            {
                await _middleware.Invoke(httpContext, _dbContext);
            }
            catch (Exception)
            {
                // Expected to throw, continue with assertions
            }

            // Assert
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseContent = new StreamReader(responseBody).ReadToEnd();

            var loggedRequest = await _dbContext.RequestLogs.FindAsync(1);

            Assert.IsNotNull(loggedRequest);
            Assert.AreEqual("GET", loggedRequest.Method);
            Assert.AreEqual("/api/error", loggedRequest.Path);
            Assert.AreEqual("?param=value", loggedRequest.QueryString);
            Assert.IsNotNull(loggedRequest.RequestBody);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, loggedRequest.StatusCode);
            Assert.AreEqual("Simulated error", loggedRequest.ResponseBody);
        }
    }
}