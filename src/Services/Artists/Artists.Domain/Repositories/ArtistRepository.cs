using Artists.Domain.Contexts;
using Artists.Domain.Objects;
using Artists.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artists.Domain.Repositories
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Artist Add(Artist order);

        void Update(Artist order);

        void Delete(int artistId);

        Task<Artist> GetAsync(int artistId);
    }

    public class ArtistRepository : IArtistRepository
    {
        private readonly ArtistContext _context;

        public ArtistRepository(ArtistContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IUnitOfWork UnitOfWork => _context;

        public Artist Add(Artist order)
        {
            throw new NotImplementedException();
        }

        public void Delete(int artistId)
        {
            throw new NotImplementedException();
        }

        public Task<Artist> GetAsync(int artistId)
        {
            throw new NotImplementedException();
        }

        public void Update(Artist order)
        {
            throw new NotImplementedException();
        }
    }
}
