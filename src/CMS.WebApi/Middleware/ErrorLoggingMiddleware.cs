using CMS.Core.Constants;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMS.WebApi.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, ex);
                throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            try
            {
                var request = context.Request;
                var now = DateTime.Now;
                var requestTime = now.ToString("yyyy/dd/mm hh:mm:ss");
                var queryPath = request.Path.ToString();
                var queryString = request.QueryString.ToString();
                var requestMethod = request.Method;
                var requestBody = await ReadBodyFromRequest(request);
                var statusCode = context.Response.StatusCode;
                var errorMessage = ex.Message;

                var errorLog = new
                {
                    requestTime,
                    queryPath,
                    queryString,
                    requestMethod,
                    requestBody,
                    statusCode,
                    errorMessage
                };

                var dir = $"{ApplicationConstants.NTTLogs}\\{now.Year}\\{now.Month.ToString("00")}";

                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var filePath = Path.Combine(dir, $"{now.ToString("yyyy-MM-dd")}-ErrorLog.json");
                using var file = File.AppendText(filePath);
                var data = JsonConvert.SerializeObject(errorLog);
                file.WriteLine(data);
            }
            catch
            {
                throw;
            }
        }

        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times 
            // (for the next middlewares in the pipeline).
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            // Reset the request's body stream position for 
            // next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
