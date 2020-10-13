using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using CK.Sprite.Framework;

namespace CK.Sprite.JobWebHost
{
    public class AbpExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public ILogger<AbpExceptionFilter> _logger { get; set; }

        public AbpExceptionFilter(ILogger<AbpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public virtual void OnException(ExceptionContext context)
        {
            HandleAndWrapException(context);
        }

        protected virtual void HandleAndWrapException(ExceptionContext context)
        {
            //TODO: Trigger an AbpExceptionHandled event or something like that.

            context.HttpContext.Response.Headers.Add("_AbpErrorFormat", "true");
            context.HttpContext.Response.StatusCode = (int)GetStatusCode(context.HttpContext, context.Exception);

            var remoteServiceErrorInfo = CreateErrorInfoWithoutCode(context.Exception);

            context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));

            _logger.LogError(context.Exception, JsonConvert.SerializeObject(remoteServiceErrorInfo));

            context.Exception = null; //Handled!
        }

        public HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
        {
            if (exception is NotImplementedException)
            {
                return HttpStatusCode.NotImplemented;
            }

            if (exception is SpriteException)
            {
                return HttpStatusCode.Forbidden;
            }

            return HttpStatusCode.InternalServerError;
        }

        protected RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            var errorInfo = new RemoteServiceErrorInfo();

            if (exception is SpriteException)
            {
                errorInfo.Message = exception.Message;
            }

            errorInfo.Message = exception.Message;

            return errorInfo;
        }
    }
}
