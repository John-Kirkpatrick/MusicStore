#region references

using Artists.Domain.Application.Config;
using MailKit.Net.Smtp;
using MimeKit;

#endregion

namespace Artists.Domain.Services {
    public interface IEmailService
    {
        #region Public Methods

        void Send(MimeMessage emailMessage);

        #endregion
    }

    public class EmailService : IEmailService {
        #region Private Fields

        private readonly IEmailConfiguration _emailConfiguration;

        #endregion

        #region Public Constructors

        public EmailService(IEmailConfiguration emailConfiguration) {
            _emailConfiguration = emailConfiguration;
        }

        #endregion

        #region Public Methods

        public void Send(MimeMessage message) {
            using (SmtpClient emailClient = new SmtpClient()) {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }

        #endregion
    }
}