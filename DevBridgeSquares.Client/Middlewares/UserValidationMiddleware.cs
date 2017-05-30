using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.ApiModels;
using DevBridgeSquares.Common.Extensions;

namespace DevBridgeSquares.Client.Middlewares
{
    public class UserValidationMiddleware
    {
        RequestDelegate _next;

        public UserValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                // TODO: LOGIN
            }
            await _next(context);
        }
    }
}
