#region references

using System.Diagnostics.CodeAnalysis;
using System.Net;
using Artists.Domain.Application.Exceptions;
using Artists.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MimeKit;

#endregion

namespace Artists.Domain.Application.Filters {
    [ExcludeFromCodeCoverage]
    public class HttpGlobalExceptionFilter : IExceptionFilter {
        #region Private Fields

        private readonly IEmailService _emailService;

        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        #endregion

        #region Public Constructors

        public HttpGlobalExceptionFilter(
            ILogger<HttpGlobalExceptionFilter> logger,
            IEmailService emailService,
            IHostingEnvironment environment
        ) {
            _logger = logger;

            _emailService = emailService;

            Environment = environment;
        }

        #endregion

        #region Private Properties

        private IHostingEnvironment Environment { get; }

        #endregion

        #region Public Methods

        public void OnException(
            ExceptionContext context
        ) {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            if (context.Exception.GetType() == typeof(ArtistDomainException)) {
                ValidationProblemDetails problemDetails = new ValidationProblemDetails {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                problemDetails.Errors.Add(
                    "DomainValidations",
                    new[] {
                        context.Exception.Message
                    }
                );

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            else {
                JsonErrorResponse json = new JsonErrorResponse {
                    Messages = new[] {
                        "An error occur.Try it again."
                    },
                    DeveloperMessage = context.Exception
                };


                // Result assigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
                // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
                _emailService.Send(GetEmailMessage(context.Exception.StackTrace));
                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        #endregion

        #region Private Methods

        private MimeMessage GetEmailMessage(
            string text
        ) {
            MimeMessage message = new MimeMessage();

            message.To.Add(new MailboxAddress(""));

            message.From.Add(new MailboxAddress(""));

            message.Subject = $"OrdersAPI Exception ({Environment.EnvironmentName})";

            TextPart body = new TextPart("plain") {
                Text = text
            };

            message.Body = body;

            return message;
        }

        #endregion

        /// <summary>
        /// </summary>
        private class JsonErrorResponse {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the messages.
            /// </summary>
            /// <value>
            ///     The messages.
            /// </value>
            public string[] Messages { get; set; }

            /// <summary>
            ///     Gets or sets the developer meesage.
            /// </summary>
            /// <value>
            ///     The developer meesage.
            /// </value>
            public object DeveloperMessage { get; set; }

            #endregion
        }
    }
}