﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vayosoft.Core.Queries;
using Warehouse.API.Resources;
using Warehouse.API.UseCases.Resources;
using Warehouse.Core.Services;
using Warehouse.Core.UseCases.Administration.Models;

namespace Warehouse.API.Controllers.API
{
    [Services.Authorization.Attributes.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityUserService _userService;
        private readonly IQueryBus _queryBus;

        public AccountController(IIdentityUserService userService, IQueryBus queryBus)
        {
            _userService = userService;
            _queryBus = queryBus;
        }

        [HttpGet("bootstrap")]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var resourceNames = new List<string> { nameof(SharedResources) };
            var resources = await _queryBus.Send(new GetResources(resourceNames), token);

            return new JsonResult(new { resources });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Post([FromBody] AuthenticateRequest model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _userService.AuthenticateAsync(model, IpAddress(), cancellationToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> PostLogout()
        {
            //await HttpContext.SignOutAsync(".session");
            HttpContext.Session.Clear();
            await Task.CompletedTask;
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRequest model, CancellationToken cancellationToken)
        {
            var refreshToken = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "Token is required" });

            var response = await _userService.RefreshTokenAsync(refreshToken, IpAddress(), cancellationToken);
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(TokenRequest model, CancellationToken cancellationToken)
        {
            // accept refresh token in request body or cookie
            var refreshToken = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "Token is required" });

            await _userService.RevokeTokenAsync(refreshToken, IpAddress(), cancellationToken);
            return Ok(new { message = "Token revoked" });
        }

        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),

                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
                Secure = true,

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                HttpOnly = true,

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                SameSite = SameSiteMode.None

                // The client should follow its default cookie policy.
                // SameSite = SameSiteMode.Unspecified
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }

    }
}
