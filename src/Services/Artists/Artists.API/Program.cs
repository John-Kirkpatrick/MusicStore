using Artists.Domain.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Artists.Domain.Application.Config;
using Infrastructure.Extensions;

namespace Artists.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<ArtistContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var settings = services.GetService<IOptions<AppSettings>>();
                    var logger = services.GetService<ILogger<ArtistContextSeed>>();

                    new ArtistContextSeed()
                    .SeedAsync(context, env, logger, settings)
                    .Wait();
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseHealthChecks("/hc")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(
                    (builderContext, config) =>
                    {
                        IHostingEnvironment env = builderContext.HostingEnvironment;
                        config.AddEnvironmentVariables();
                        config.AddJsonFile("appsettings.json", true, true);

                        if (env.EnvironmentName == "QA" || env.EnvironmentName == "Staging")
                        {
                            config.AddJsonFile("appsettings.Staging.json", true, true);
                        }
                        else
                        {
                            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                        }
                    }
                )
                .ConfigureLogging(
                    (hostingContext, builder) =>
                    {
                        builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        builder.AddDebug();
                        builder.AddConsole();
                    }
                )
                .Build();
    
    }
}
