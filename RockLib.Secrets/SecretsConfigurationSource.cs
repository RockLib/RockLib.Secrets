using Microsoft.Extensions.Configuration;
using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// Represents a source of secrets as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class SecretsConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationSource"/> class.
        /// </summary>
        /// <param name="secretsProvider"></param>
        public SecretsConfigurationSource(ISecretsProvider secretsProvider) =>
            SecretsProvider = secretsProvider ?? throw new ArgumentNullException(nameof(secretsProvider));

        /// <summary>
        /// Gets the <see cref="ISecretsProvider"/>.
        /// </summary>
        public ISecretsProvider SecretsProvider { get; }

        /// <summary>
        /// Builds the <see cref="SecretsConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An instance of <see cref="SecretsConfigurationProvider"/>.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new SecretsConfigurationProvider(SecretsProvider);
    }
}
