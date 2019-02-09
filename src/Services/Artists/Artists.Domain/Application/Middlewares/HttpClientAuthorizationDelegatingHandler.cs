#region references

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Artists.Domain.Application.Config;
using IdentityModel.Client;
using Infrastructure.Caching;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

#endregion

namespace Artists.Domain.Application.Middlewares {
    /// <summary>
    ///     Used to capture the incoming authentication token and relay it to subsequent client calls
    ///     to other API resources
    /// </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler {
        #region Static Fields

        private const string AccessToken = "access_token";

        private const string Cachekey = "HttpClientAuthorizationDelegatingHandlerBearerToken";

        #endregion

        #region Private Fields

        private readonly IETagCache _cache;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IOptionsSnapshot<AppSettings> _settings;

        #endregion

        #region Public Constructors

        public HttpClientAuthorizationDelegatingHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<AppSettings> settings,
            IETagCache cache
        ) {
            _httpContextAccessor = httpContextAccessor;
            InnerHandler = new HttpClientHandler();
            _settings = settings;
            _cache = cache;
        }

        #endregion

        #region Private Methods

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) {
            StringValues authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader)) {
                request.Headers.Add(
                    "Authorization",
                    new List<string> {
                        authorizationHeader
                    }
                );
            }

            string token = await GetToken();

            //if there is no token on the httprequest (typically test scenarios or endpoints with allowanonymous)
            if (string.IsNullOrEmpty(token)) {
                token = await CacheTryGetAccessToken();
            }

            if (token != null) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Retrieves an access token is attached to authentication header
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken() {
            string token = null;

            try {
                token = await _httpContextAccessor.HttpContext.GetTokenAsync(AccessToken);
            }
            catch (Exception exc) {
                //exception here should only occur when using the test startup
                //which does not enable authentication
                string msg = exc.GetBaseException().Message;
            }

            return token;
        }

        /// <summary>
        /// Contacts identity to retrieve an access token via client secret
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetTokenBasicAuthAsync() {
            //retrieve token via basic auth
            HttpClient client = new HttpClient();

            TokenResponse response = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest {
                    Address = $"{_settings.Value.IdentityUrl}/connect/token",
                    ClientId = "eventscoreui",
                    ClientSecret = "secret",
                    Scope = "eventsapi ordersapi constituentsapi"
                }
            );

            return !string.IsNullOrEmpty(response.AccessToken) ? response.AccessToken : null;
        }

        /// <summary>
        ///     https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2
        /// </summary>
        /// <returns></returns>
        private async Task<string> CacheTryGetAccessToken() {
            string cacheEntry = _cache.GetCachedObject<string>(Cachekey);

            // Look for cache key.
            if (cacheEntry != null) {
                return cacheEntry;
            }

            cacheEntry = await GetTokenBasicAuthAsync();

            if (string.IsNullOrEmpty(cacheEntry)) {
                return null;
            }

            // Set cache options.
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60), SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            // Save data in cache.
            _cache.SetCachedObject(Cachekey, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }

        #endregion
    }
}