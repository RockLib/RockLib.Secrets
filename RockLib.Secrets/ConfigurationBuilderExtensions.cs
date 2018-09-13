using Microsoft.Extensions.Configuration;
using RockLib.Configuration.ObjectFactory;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    public static class ConfigurationBuilderExtensions
    {
        public const string DefaultSectionName = "RockLib.Secrets";

        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder) =>
            builder.AddRockLibSecrets(DefaultSectionName);

        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder, string sectionName) =>
            builder.AddRockLibSecrets(CreateSecretsProvider(builder.Build(), sectionName));

        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder, ISecretsProvider secretsProvider) =>
            builder.Add(new SecretsConfigurationSource(secretsProvider));

        private static ISecretsProvider CreateSecretsProvider(IConfiguration configuration, string sectionName)
        {
            List<ISecretsProvider> secretsProviders;
            try { secretsProviders = configuration.GetSection(sectionName).Create<List<ISecretsProvider>>(); }
            catch { return new NullSecretsProvider(); } // TODO: or throw an exception? or don't catch the exception? 
            if (secretsProviders.Count == 0)
                return new NullSecretsProvider(); // TODO: or throw an exception?
            if (secretsProviders.Count == 1)
                return secretsProviders[0];
            return new CompositeSecretsProvider(secretsProviders);
        }
    }
}
