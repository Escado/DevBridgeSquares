using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.ApiModels;
using DevBridgeSquares.Common.Extensions;

namespace RDS.API.Middlewares
{
    public class ExceptionMiddleware
    {
        RequestDelegate _next;
        private ILogger Logger { get; }

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().Name);
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DVBSException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                SetResponseBody(context, new ApiErrorResponse()
                {
                    code = Convert.ToInt32(ex.Code),
                    field = ex.Field,
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                //TODO: log
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = new ApiErrorResponse()
                {
                    code = (int)DVBSCode.General.UnknownError,
                    error = DVBSCode.General.UnknownError.GetDescription(),
                };

                response.error = ex.Message;
                response.stackTrace = ex.StackTrace;

                SetResponseBody(context, response);
            }
        }

        public void SetResponseBody(HttpContext context, ApiErrorResponse response)
        {
            var json = JsonConvert.SerializeObject(response);
            using (var streamWriter = new StreamWriter(context.Response.Body))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
        }
    }
}
