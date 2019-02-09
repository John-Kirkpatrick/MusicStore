using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artists.Domain.Contexts;
using Artists.Domain.Objects;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;

namespace Artists.Test.Generators
{
    public class ArtistsDataGenerator : IDataGenerator
    {
        private ArtistContext _context;
        private List<Artist> _artists;

        public ArtistsDataGenerator(DbContextOptions<ArtistContext> options = null)
        {
            options = options ?? GetDefaultOptions();
            _context = new ArtistContext(options);
        }

        public void Dispose()
        {
            _artists = null;
            _context.Dispose();
            _context = null;
        }

        public ArtistContext GenerateMockContext(IGeneratorTemplate template)
        {
            if (template.ArtistCount > 0)
            {
                _artists = Builder<Artist>
                                .CreateListOfSize(template.ArtistCount)
                                .All()
                                //.With(e => e.LookupValueId = )
                                .Build()
                                .ToList();
                _context.Artists.AddRange(_artists);
            }

            _context.SaveChanges();

            return _context;
        }

        private static DbContextOptions<ArtistContext> GetDefaultOptions()
        {
            return new DbContextOptionsBuilder<ArtistContext>().UseInMemoryDatabase("ArtistMemoryContext").Options;
        }
    }
}
