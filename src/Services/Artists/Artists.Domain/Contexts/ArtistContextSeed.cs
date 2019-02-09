using Artists.Domain.Application.Config;
using Artists.Domain.Objects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artists.Domain.Contexts
{
    public class ArtistContextSeed
    {
        public async Task SeedAsync(ArtistContext context, 
                                    IHostingEnvironment env,
                                    ILogger<ArtistContextSeed> logger, 
                                    IOptions<AppSettings> settings, 
                                    int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                if (!context.Artists.Any())
                {
                    context.Artists.AddRange(SetDefaultArtists());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;

                    logger.LogError(ex.Message, $"There is an error migrating data for ArtistContext");

                    await SeedAsync(context, env, logger, settings, retryForAvailability);
                }
            }
        }

        private IEnumerable<Artist> SetDefaultArtists()
        {
            Artist artist1 = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = "Queen"
            };

            Artist artist2 = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = "David Bowie"
            };

            Artist artist3 = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = "Rolling Stones"
            };

            Artist artist4 = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = "Radiohead"
            };

            Artist artist5 = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = "Beatles"
            };

            return new List<Artist>()
            {
                artist1,
                artist2,
                artist3,
                artist4,
                artist5
            };
        }
    }
}