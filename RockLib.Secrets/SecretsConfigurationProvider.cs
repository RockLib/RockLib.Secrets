using Microsoft.Extensions.Configuration;

namespace RockLib.Secrets
{
    public class SecretsConfigurationProvider : ConfigurationProvider
    {
        public SecretsConfigurationProvider(ISecretsProvider secretsProvider)
        {
            foreach (var secret in secretsProvider.GetSecrets())
                Data.Add(secret.Key, secret.GetValue());
        }
    }
}
