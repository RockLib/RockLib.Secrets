using Microsoft.Extensions.Configuration;

namespace RockLib.Secrets
{
    /// <summary>
    /// Represents a source of secrets as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class SecretsConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Used to access a collection of secrets. If null, the default secrets provider,
        /// stored in the configuration builder's properties, will be used instead.
        /// </summary>
        public ISecretsProvider SecretsProvider { get; set; }

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
            SecretsProvider = SecretsProvider ?? builder.GetSecretsProvider();
        }
    }
}
