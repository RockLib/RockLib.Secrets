using Microsoft.Extensions.Configuration;

namespace RockLib.Secrets
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationProvider"/> backed by
    /// a secret store.
    /// </summary>
    public class SecretsConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationProvider"/> class.
        /// </summary>
        /// <param name="secretsProvider">
        /// An object that provides the secrets that make up the settings of this instance.
        /// </param>
        public SecretsConfigurationProvider(ISecretsProvider secretsProvider)
        {
            foreach (var secret in secretsProvider.GetSecrets())
            {
                string value;
                try { value = secret.GetValue(); }
                catch { continue; }
                Data.Add(secret.Key, value);
            }
        }
    }
}
