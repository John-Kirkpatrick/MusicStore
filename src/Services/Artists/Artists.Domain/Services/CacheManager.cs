using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Artists.Domain.Objects;

namespace Artists.Domain.Services
{
    public interface ICacheManager {
        Task ClearByType(
            List<AggregateRoot> type
        );

        Task GetKeysByType(
            List<AggregateRoot> type
        );
    }

    public class CacheManager : ICacheManager
    {
        public Task ClearByType(
            List<AggregateRoot> type
        ) {
            throw new NotImplementedException();
        }

        public Task GetKeysByType(
            List<AggregateRoot> type
        ) {
            throw new NotImplementedException();
        }
    }
}
