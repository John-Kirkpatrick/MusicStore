#region references

using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#endregion

namespace Artists.Domain.Application.Filters
{
    [ExcludeFromCodeCoverage]
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        #region Public Methods

        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            bool hasAuthorize = context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Any() ||
                                context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add(
                    "401",
                    new Response
                    {
                        Description = "Unauthorized"
                    }
                );
                operation.Responses.Add(
                    "403",
                    new Response
                    {
                        Description = "Forbidden"
                    }
                );

                operation.Security = new List<IDictionary<string, IEnumerable<string>>> {
                    new Dictionary<string, IEnumerable<string>> {
                        {
                            "oauth2", new[] {
                                "ordersapi"
                            }
                        }
                    }
                };
            }
        }

        #endregion
    }
}