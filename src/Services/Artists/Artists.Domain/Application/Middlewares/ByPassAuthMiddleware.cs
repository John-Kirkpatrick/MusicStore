﻿#region references

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

#endregion

namespace Artists.Domain.Application.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ByPassAuthMiddleware
    {
        #region Private Fields

        private readonly RequestDelegate _next;
        private string _currentUserId;

        #endregion

        #region Public Constructors

        public ByPassAuthMiddleware(RequestDelegate next)
        {
            _next = next;
            _currentUserId = null;
        }

        #endregion

        #region Public Methods

        public async Task Invoke(HttpContext context) {
            PathString path = context.Request.Path;
            switch (path) {
                case "/noauth": {
                    StringValues userid = context.Request.Query["userid"];
                    if (!string.IsNullOrEmpty(userid))
                    {
                        _currentUserId = userid;
                    }

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/string";
                    await context.Response.WriteAsync($"User set to {_currentUserId}");
                    break;
                }
                case "/noauth/reset":
                    _currentUserId = null;
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/string";
                    await context.Response.WriteAsync("User set to none. Token required for protected endpoints.");
                    break;
                default: {
                    string currentUserId = _currentUserId;

                    StringValues authHeader = context.Request.Headers["Authorization"];
                    if (authHeader != StringValues.Empty)
                    {
                        string header = authHeader.FirstOrDefault();
                        if (!string.IsNullOrEmpty(header) &&
                            header.StartsWith("Email ") &&
                            header.Length > "Email ".Length)
                        {
                            currentUserId = header.Substring("Email ".Length);
                        }
                    }


                    if (!string.IsNullOrEmpty(currentUserId))
                    {
                        ClaimsIdentity user = new ClaimsIdentity(
                            new[] {
                                new Claim("emails", currentUserId),
                                new Claim("name", "Test user"),
                                new Claim("nonce", Guid.NewGuid().ToString()),
                                new Claim(
                                    "ttp://schemas.microsoft.com/identity/claims/identityprovider",
                                    "ByPassAuthMiddleware"
                                ),
                                new Claim("nonce", Guid.NewGuid().ToString()),
                                new Claim("sub", "1234"),
                                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "User"),
                                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "Microsoft")
                            },
                            "ByPassAuth"
                        );

                        context.User = new ClaimsPrincipal(user);
                    }

                    await _next.Invoke(context);
                    break;
                }
            }
        }

        #endregion
    }
}