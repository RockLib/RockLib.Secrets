using System;

namespace RockLib.Secrets
{
    public class SecretsConfigurationBuilder : ISecretsConfigurationBuilder
    {
        public SecretsConfigurationBuilder(SecretsConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public SecretsConfigurationSource Source { get; }

        public ISecretsConfigurationBuilder AddSecret(ISecret secret)
        {
            Source.Secrets.Add(secret);
            return this;
        }
    }
}
