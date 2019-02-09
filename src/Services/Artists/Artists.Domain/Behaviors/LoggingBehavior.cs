#region references

using Artists.Domain.Objects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Artists.Domain.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IntegrationCommand
    {
        #region Private Fields

        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        #endregion

        #region Public Constructors

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            TResponse response = await next();
            _logger.LogInformation($"Handled {typeof(TResponse).Name}");
            return response;
        }

        #endregion
    }
}