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

        private ArtistContext _context;

        public async Task SeedAsync(ArtistContext context, 
                                    IHostingEnvironment env,
                                    ILogger<ArtistContextSeed> logger, 
                                    IOptions<AppSettings> settings, 
                                    int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            _context = context ?? throw new ArgumentException($"{nameof(ArtistContext)} is required");

            try
            {
                if (!context.Bands.Any())
                {
                    context.Bands.AddRange(SetDefaultBands());
                    await context.SaveChangesAsync();
                }

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

        private IEnumerable<Band> SetDefaultBands()
        {
            Band band1 = new Band()
            {
                Id = Guid.NewGuid(),
                Name = "Queen"
            };

            Band band2 = new Band()
            {
                Id = Guid.NewGuid(),
                Name = "David Bowie"
            };

            Band band3 = new Band()
            {
                Id = Guid.NewGuid(),
                Name = "Rolling Stones"
            };

            Band band4 = new Band()
            {
                Id = Guid.NewGuid(),
                Name = "Radiohead"
            };

            Band band5 = new Band()
            {
                Id = Guid.NewGuid(),
                Name = "Beatles"
            };

            return new List<Band>()
            {
                band1,
                band2,
                band3,
                band4,
                band5
            };
        }

        private IEnumerable<Artist> SetDefaultArtists()
        {
            Artist artist1 = new Artist()
            {
                Id = Guid.NewGuid(),
                FirstName = "Freddy",
                LastName = "Mercury",
                BandId = _context.Bands.First(x => x.Name == "Queen").Id
            };

            Artist artist2 = new Artist()
            {
                Id = Guid.NewGuid(),
                FirstName = "Robbie",
                LastName = "Williams",
                BandId = _context.Bands.First(x => x.Name == "Queen").Id
            };

            Artist artist3 = new Artist()
            {
                Id = Guid.NewGuid(),
                FirstName = "Mick",
                LastName = "Jagger",
                BandId = _context.Bands.First(x => x.Name == "Rolling Stones").Id
            };

            Artist artist4 = new Artist()
            {
                Id = Guid.NewGuid(),
                FirstName = "Paul",
                LastName = "McCartney",
                BandId = _context.Bands.First(x => x.Name == "Rolling Stones").Id
            };

            Artist artist5 = new Artist()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Lennon",
                BandId = _context.Bands.First(x => x.Name == "Rolling Stones").Id
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