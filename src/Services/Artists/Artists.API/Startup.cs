using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Artists.Domain;
using Artists.Domain.Application.Config;
using Artists.Domain.Application.Configuration;
using Artists.Domain.Application.Constants;
using Artists.Domain.Application.Middlewares;
using Artists.Domain.Behaviors;
using Artists.Domain.Contexts;
using Artists.Domain.Services;
using AutoMapper;
using Infrastructure.Caching;
using Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.Swagger;

namespace Artists.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //set 5 min as the lifetime for each HttpMessageHandler int the pool
            services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //register delegating handlers
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddHttpClient<IEventApiClient, EventApiClient>()
            //    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Sample. Default lifetime is 2 minutes
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //    .AddPolicyHandler(GetRetryPolicy())
            //    .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddMvc(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(
                    options => {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    }
                )
                .AddControllersAsServices();

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddDbContext<ArtistContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString(Keys.ArtistContextConnectionString)));

            services.AddDbContext<ArtistContext>(
              options => options.UseSqlServer(
                  Configuration.GetConnectionString(Keys.ArtistContextConnectionString),
                  sqlOptions => sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null)
              ),
              ServiceLifetime.Scoped);

            services.Configure<AppSettings>(Configuration)
           .AddMemoryCache()
           .AddScoped<IETagCache, ETagCache>() //https://www.carlrippon.com/scalable-and-performant-asp-net-core-web-apis-server-caching/
           .AddSingleton(Environment)
           .AddTransient<IIdentityService, IdentityService>()
           //.AddScoped<IEventApiClient, EventApiClient>()
           .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
           .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
           .AddTransient<IEmailService, EmailService>()
           .AddMediatR();

            //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-2.1#distributed-sql-server-cache
            services.AddDistributedSqlServerCache(
                options => {
                    options.ConnectionString = Configuration.GetConnectionString(Keys.DistributedCacheConnectionName);
                    options.SchemaName = Caching.CachingSchema;
                    options.TableName = Caching.CachingTable;
                }
            );

            services.AddSingleton<IEmailConfiguration>(
                Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>()
            );

            AddCustomAuthentication(services);
            AddCustomSwagger(services);

            //automapper config
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            if (Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString)
            {
                services.AddDataProtection(opts => opts.ApplicationDiscriminator = "ice.orders");
            }

            //disabling per sprint refinement 11-20-2018
            //if (Configuration.GetValue<string>("EnablePaymentUpdateReport") == bool.TrueString)
            //{
            //    services.AddHostedService<PaymentReportHostedService>();
            //}

            const int defaultCheckInterval = 5;
            services.AddHealthChecks(
                checks => {
                    int minutes = defaultCheckInterval;

                    //see if configuration is over set, if so use configured setting
                    if (int.TryParse(Configuration["HealthCheck:Timeout"], out int minutesParsed))
                    {
                        minutes = minutesParsed;
                    }

                    checks.AddSqlCheck(
                        Keys.ArtistContextConnectionString,
                        Configuration.GetConnectionString(Keys.ArtistContextConnectionString),
                        TimeSpan.FromMinutes(minutes)
                    );
                }
            );

            services.AddLogging(configure => configure.AddConsole());
            services.AddAutoMapper();

            services.AddCors(
                        options => options.AddPolicy(
                            "Everything",
                            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                        )
                    )
                    .AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artist.API V1"));

            app.UseHttpsRedirection();
            app.UseCors("Everything");
            ConfigureAuth(app);
            app.UseResponseCaching();
            app.UseMvc();
        }

        #region Private Methods

        protected virtual IServiceCollection AddCustomSwagger(IServiceCollection services)
        {
            Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Startup));

            services.AddSwaggerGen(
                options => {
                    options.DescribeAllEnumsAsStrings();
                    options.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "Artist.API - " + Environment.EnvironmentName + " Environment",
                            Version = "v1",
                            Description = $"The Artist HTTP API {assembly.FullName}"
                        }
                    );

                    options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize",
                        TokenUrl = $"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token",
                        Scopes = new Dictionary<string, string>()
                        {
                            { "artistapi", "Artist API" }
                        }
                    });

                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                }
            );

            return services;
        }

        protected virtual void AddCustomAuthentication(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            string identityUrl = Configuration.GetValue<string>("IdentityUrl");

            if (string.IsNullOrEmpty(identityUrl))
            {
                throw new Exception("invalid identity url");
            }

            services.AddAuthentication("Bearer")
                    .AddJwtBearer(
                        options => {
                            // base-address of your identityserver
                            options.Authority = identityUrl;
                            options.RequireHttpsMetadata = false;
                            // name of the API resource
                            options.Audience = "artistapi";
                        }
                    );
        }

        //to be overridden by test startup
        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
        }

        #endregion

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                   .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
