using Artists.Domain.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artists.Domain.Contexts
{
    public class ArtistContext : DbContext
    {
        public ArtistContext(DbContextOptions<ArtistContext> options)
            : base(options)
        { }

        public DbSet<Artist> Artists { get; set; }
    }
}
