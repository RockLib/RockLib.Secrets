using Microsoft.Extensions.Configuration;

namespace RockLib.Secrets
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddRockLibSecrets(this IConfigurationBuilder builder, string sectionName = "RockLib.Secrets") =>
            builder.Add(new SecretsConfigurationSource(builder.Build(), sectionName));

        public static void AddRockLibSecrets(this IConfigurationBuilder builder, ISecretsProvider secretsProvider) =>
            builder.Add(new SecretsConfigurationSource(secretsProvider));
    }
}
