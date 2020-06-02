using Microsoft.Extensions.Configuration;
using System;
using System.Threading;

namespace RockLib.Secrets
{
    /// <summary>
    /// Represents a source of secrets as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class SecretsConfigurationSource : IConfigurationSource
    {
        /// <summary>The default value of the <see cref="ReloadMilliseconds"/> property, 5 minutes.</summary>
        public const int DefaultReloadMilliseconds = 5 * 60 * 1000;

        private int _reloadMilliseconds = DefaultReloadMilliseconds;

        /// <summary>
        /// Used to access a collection of secrets. If null, the default secrets provider,
        /// stored in the configuration builder's properties, will be used instead.
        /// </summary>
        public ISecretsProvider SecretsProvider { get; set; }

        /// <summary>
        /// Will be called if an uncaught exception occurs when calling <see cref="ISecret.GetValue"/>
        /// from the <see cref="SecretsConfigurationProvider.Load"/> method.
        /// </summary>
        public Action<SecretExceptionContext> OnSecretException { get; set; }

        /// <summary>
        /// The number of milliseconds to wait before reloading secrets. Specify <see cref="Timeout.Infinite"/>
        /// to disable reloading.
        /// </summary>
        public int ReloadMilliseconds
        {
            get => _reloadMilliseconds;
            set
            {
                if (value < Timeout.Infinite)
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be either non-negative or -1.");
                _reloadMilliseconds = value;
            }
        }

        /// <summary>
        /// Disables periodic reloading.
        /// </summary>
        public void DisableReload() => ReloadMilliseconds = Timeout.Infinite;

        /// <summary>
        /// Builds the <see cref="SecretsConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An instance of <see cref="SecretsConfigurationProvider"/>.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new SecretsConfigurationProvider(this);
        }

        private void EnsureDefaults(IConfigurationBuilder builder)
        {
            SecretsProvider = SecretsProvider ?? builder.GetSecretsProvider() ?? throw new InvalidOperationException("No secrets provider was provided.");
            OnSecretException = OnSecretException ?? builder.GetSecretExceptionHandler();
        }
    }
}
