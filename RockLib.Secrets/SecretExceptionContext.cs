﻿using System;

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
        /// <param name="provider">
        /// The <see cref="SecretsConfigurationProvider"/> that invoked the <see cref="ISecret.GetValue"/>
        /// method that caused the exception.
        /// </param>
        /// <param name="secret">The <see cref="ISecret"/> that caused the exception.</param>
        /// <param name="exception">The exception that occurred in <see cref="ISecret.GetValue"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="provider"/> or <paramref name="secret"/> or <paramref name="exception"/> is <c>null</c>.
        /// </exception>
        public SecretExceptionContext(SecretsConfigurationProvider provider, ISecret secret, Exception exception)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        /// <summary>
        /// The <see cref="SecretsConfigurationProvider"/> that invoked the <see cref="ISecret.GetValue"/>
        /// method that caused the exception.
        /// </summary>
        public SecretsConfigurationProvider Provider { get; }

        /// <summary>
        /// The <see cref="ISecret"/> that caused the exception.
        /// </summary>
        public ISecret Secret { get; }

        /// <summary>
        /// The exception that occurred in <see cref="ISecret.GetValue"/>.
        /// </summary>
        public Exception Exception { get; }
    }
}