#region references

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Artists.Domain.Objects;
using Artists.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

#endregion

namespace Artists.Domain.Behaviors {
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IntegrationCommand {
        #region Private Fields

        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        private readonly ICacheManager _manager;

        #endregion

        #region Public Constructors

        public CachingBehavior(
            ICacheManager manager,
            ILogger<CachingBehavior<TRequest, TResponse>> logger
        ) {
            _manager = manager;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            TResponse response = await next();

            List<AggregateRoot> types = GetAggregateRootTypes(request);
            await _manager.ClearByType(types);
            return response;
        }

        #endregion

        #region Private Methods

        private List<AggregateRoot> GetAggregateRootTypes(
            TRequest request
        ) {
            // get all public static properties of MyClass type
            List<PropertyInfo> propertyInfos = typeof(TRequest).GetRuntimeProperties().ToList();

            List<AggregateRoot> cacheTypes = new List<AggregateRoot>();
            //determine which types need to be cleared from cache based on properties of command being logged
            if (propertyInfos.Select(x => x.Name).Contains("EventId")) {
                cacheTypes.Add(AggregateRoot.Event);
            }

            if (propertyInfos.Select(x => x.Name).Contains("ConstituentId")) {
                cacheTypes.Add(AggregateRoot.Constituent);
            }

            if (propertyInfos.Select(x => x.Name).Contains("OrderId")) {
                cacheTypes.Add(AggregateRoot.Order);
            }

            return cacheTypes;
        }

        #endregion
    }
}