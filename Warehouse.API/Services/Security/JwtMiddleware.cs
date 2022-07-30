﻿using Microsoft.Extensions.Options;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Security.Session;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Services;
using Warehouse.Core.UseCases.Administration.Models;

namespace Warehouse.API.Services.Security
{
    //https://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?cid=kerryherger&view=aspnetcore-6.0
    //https://metanit.com/sharp/aspnet5/2.26.php?ysclid=l67iov921a229435244
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IIdentityUserService userService, IJwtService jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var cancellationToken = context.RequestAborted;
                var userId = jwtUtils.ValidateJwtToken(token);
                if (userId != null)
                {
                    IdentityData identity;
                    if (context.Session.Keys.Contains(nameof(IdentityData)))
                        identity = context.Session.Get<IdentityData>(nameof(IdentityData));
                    else
                    {
                        var user = (UserEntity)await userService.GetByIdAsync(userId.Value, cancellationToken);
                        identity = new IdentityData(user.Id, user.ProviderId);
                        context.Session.Set(nameof(IdentityData), identity);
                    }
                    // attach user to context on successful jwt validation
                    context.Items["User"] = await userService.GetByIdAsync(userId.Value, cancellationToken);
                }
            }

            await _next(context);
        }
    }
}
