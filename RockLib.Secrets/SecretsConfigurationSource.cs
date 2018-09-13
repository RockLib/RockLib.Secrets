using Microsoft.Extensions.Configuration;
using System;

namespace RockLib.Secrets
{
    public class SecretsConfigurationSource : IConfigurationSource
    {
        public SecretsConfigurationSource(ISecretsProvider secretsProvider) =>
            SecretsProvider = secretsProvider ?? throw new ArgumentNullException(nameof(secretsProvider));

        public ISecretsProvider SecretsProvider { get; }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new SecretsConfigurationProvider(SecretsProvider);
    }
}
