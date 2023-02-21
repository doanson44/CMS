using System;
using CMS.Core.Enums;
using CMS.Core.Exceptions;
using CMS.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CMS.WebApi.Middleware;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";

                var error = context.Features.Get<IExceptionHandlerFeature>();
                var logger = loggerFactory.CreateLogger("BusinessException");
                if (error != null)
                {
                    var ex = error.Error;
                    var code = ErrorCodes.InternalServerError;
                    var msg = ex.Message + Environment.NewLine + ex.InnerException?.Message + Environment.NewLine + ex.StackTrace;

                    if (ex.GetType() == typeof(BusinessException))
                    {
                        code = ((BusinessException)ex).StatusCode;
                        msg = ex.Message;

                        logger.LogWarning($"{msg}");
                    }
                    else if (ex.GetType() == typeof(EntityNotFoundException) || ex.GetType() == typeof(ArgumentNullException))
                    {
                        code = ErrorCodes.EntityNotFound;
                    }
                    else if (ex.GetType() == typeof(ArgumentException))
                    {
                        code = ErrorCodes.BadRequest;
                    }

                    await context.Response.WriteAsync(new BaseResponseModel
                    {
                        Code = code,
                        Message = msg
                    }.ToString());
                }
            });
        });

        return app;
    }
}
