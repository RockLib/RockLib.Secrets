using Microsoft.Extensions.Configuration;
using RockLib.Configuration.ObjectFactory;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    /// <summary>
    /// Extension methods for adding <see cref="SecretsConfigurationProvider"/>.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds the secrets configuration provider to the <paramref name="configurationBuilder"/>.
        /// <para>
        /// The underlying implementation of <see cref="ISecretsProvider"/> is created by building the
        /// current value of the <paramref name="configurationBuilder"/> parameter and using the value of
        /// its "RockLib.Secrets" configuration section to create the instance.
        /// </para>
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder configurationBuilder) =>
            configurationBuilder.AddRockLibSecrets(CreateSecretsProvider(configurationBuilder.Build()));

        /// <summary>
        /// Adds the secrets configuration provider to the <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="secretsProvider">
        /// The implementation of <see cref="ISecretsProvider"/> that provides the secrets that are mapped
        /// to configuration settings.
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder configurationBuilder,
            ISecretsProvider secretsProvider) =>
            configurationBuilder.Add(new SecretsConfigurationSource(secretsProvider));

        private static ISecretsProvider CreateSecretsProvider(IConfiguration configuration)
        {
            List<ISecretsProvider> secretsProviders;
            try { secretsProviders = configuration.GetSection("RockLib.Secrets").Create<List<ISecretsProvider>>(); }
            catch { return new NullSecretsProvider(); } // TODO: or throw an exception? or don't catch the exception? 
            if (secretsProviders.Count == 0)
                return new NullSecretsProvider(); // TODO: or throw an exception?
            if (secretsProviders.Count == 1)
                return secretsProviders[0];
            return new CompositeSecretsProvider(secretsProviders);
        }
    }
}
