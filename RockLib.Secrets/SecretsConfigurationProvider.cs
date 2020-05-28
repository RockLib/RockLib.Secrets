using Microsoft.Extensions.Configuration;
using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationProvider"/> backed by
    /// a <see cref="ISecretsProvider"/>.
    /// </summary>
    public class SecretsConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationProvider"/> class.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public SecretsConfigurationProvider(SecretsConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public SecretsConfigurationSource Source { get; }

        /// <summary>
        /// Loads data from the secrets provider.
        /// </summary>
        public override void Load()
        {
            foreach (var secret in Source.SecretsProvider.Secrets)
            {
                string value;
                try { value = secret.GetValue(); }
                catch { continue; }
                Data.Add(secret.Key, value);
            }
        }
    }
}
