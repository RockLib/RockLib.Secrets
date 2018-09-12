using Microsoft.Extensions.Configuration;
using RockLib.Configuration.ObjectFactory;
using System;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    public class SecretsConfigurationSource : IConfigurationSource
    {
        private readonly ISecretsProvider _secretsProvider;

        public SecretsConfigurationSource(IConfiguration configuration, string sectionName)
            : this(CreateSecretsProvider(configuration, sectionName))
        {
        }

        public SecretsConfigurationSource(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider ?? throw new ArgumentNullException(nameof(secretsProvider));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new SecretsConfigurationProvider(_secretsProvider);

        private static ISecretsProvider CreateSecretsProvider(IConfiguration configuration, string sectionName)
        {
            var secretsProviders = configuration.GetSection("RockLib.Secrets").Create<List<ISecretsProvider>>();
            if (secretsProviders.Count == 0)
                return new NullSecretsProvider(); // TODO: or throw exception?
            if (secretsProviders.Count == 1)
                return secretsProviders[0];
            return new CompositeSecretsProvider(secretsProviders);
        }
    }
}
