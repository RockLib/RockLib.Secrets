using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// Contains information about a secret exception.
    /// </summary>
    public class SecretExceptionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretExceptionContext"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="SecretsConfigurationProvider"/> that caused the exception.</param>
        /// <param name="exception">The exception that occurred in <see cref="ISecret.GetValue"/>.</param>
        public SecretExceptionContext(SecretsConfigurationProvider provider, Exception exception)
        {
            Provider = provider;
            Exception = exception;
        }

        /// <summary>
        /// The <see cref="SecretsConfigurationProvider"/> that caused the exception.
        /// </summary>
        public SecretsConfigurationProvider Provider { get; }

        /// <summary>
        /// The exception that occurred in <see cref="ISecret.GetValue"/>.
        /// </summary>
        public Exception Exception { get; }
    }
}
