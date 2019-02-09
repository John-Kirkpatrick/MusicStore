#region references

using Artists.Domain.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Artists.Domain.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        #region Private Fields

        private readonly IValidator<TRequest>[] _validators;

        #endregion

        #region Public Constructors

        public ValidatorBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public ValidatorBehavior(IValidator<TRequest> validator)
        {
            _validators = new IValidator<TRequest>[1] {
                validator
            };
        }

        #endregion

        #region Public Methods

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            List<ValidationFailure> failures = _validators
                                               .Select(v => v.Validate(request))
                                               .SelectMany(result => result.Errors)
                                               .Where(error => error != null)
                                               .ToList();

            if (failures.Any())
            {
                throw new ArtistDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}",
                    new ValidationException("Validation exception", failures)
                );
            }

            TResponse response = await next();
            return response;
        }

        #endregion
    }
}