#region references

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Artists.Domain.Application.Config {
    [ExcludeFromCodeCoverage]
    public class AppSettings {
        #region Public Properties

        /// <summary>
        /// </summary>
        public virtual string AesWebAPI { get; set; }

        /// <summary>
        /// </summary>
        public virtual string AesApiKey { get; set; }

        /// <summary>
        /// </summary>
        public virtual string VirtualFolder { get; set; }

        public virtual bool EnablePaymentUpdateReport { get; set; }

        public virtual string PaymentUpdateReportRecipients { get; set; }

        /// <summary>
        /// </summary>
        public Connectionstrings ConnectionStrings { get; set; }

        /// <summary>
        /// </summary>
        public virtual string SignalrUri { get; set; }

        /// <summary>
        /// </summary>
        public Logging Logging { get; set; }

        /// <summary>
        /// </summary>
        public bool UseCustomizationData { get; set; }

        /// <summary>
        /// </summary>

        public bool UseResilientHttp { get; set; }

        /// <summary>
        /// </summary>
        public bool UseLoadTest { get; set; }

        /// <summary>
        /// </summary>
        public int HttpClientRetryCount { get; set; }

        /// <summary>
        /// </summary>
        public int HttpClientExceptionsAllowedBeforeBreaking { get; set; }

        public virtual string ConstituentApiUri { get; set; }

        public virtual string EventApiUri { get; set; }

        public virtual string ConvioApiUri { get; set; }

        public virtual string ConvioApiKey { get; set; }

        public virtual string ConvioApiUsername { get; set; }

        public virtual string ConvioApiPassword { get; set; }

        public virtual string ConvioApiVersion { get; set; }

        public virtual string IdentityUrl { get; set; }

        public virtual bool EnableDistributedCaching { get; set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class Connectionstrings {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string DefaultConnection { get; set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class Logging {
        #region Public Properties

        /// <summary>
        /// </summary>
        public bool IncludeScopes { get; set; }

        /// <summary>
        /// </summary>
        public Loglevel LogLevel { get; set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class Loglevel {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// </summary>
        public string Microsoft { get; set; }

        #endregion
    }

    public interface IEmailConfiguration {
        #region Public Properties

        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string PopServer { get; }
        int PopPort { get; }
        string PopUsername { get; }
        string PopPassword { get; }

        #endregion
    }

    public class EmailConfiguration : IEmailConfiguration {
        #region Public Properties

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }

        #endregion
    }
}