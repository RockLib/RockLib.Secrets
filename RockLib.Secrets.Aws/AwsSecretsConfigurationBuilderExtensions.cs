using Amazon.SecretsManager;

namespace RockLib.Secrets.Aws
{
    /// <summary>
    /// Extension methods for secrets configuration builders related to AWS.
    /// </summary>
    public static class AwsSecretsConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds an <see cref="AwsSecret"/> to the <see cref="ISecretsConfigurationBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder the secret is beind added to.</param>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="awsSecretKey">The key of the secret in AWS.</param>
        /// <param name="secretsManager">The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.</param>
        /// <returns>The same <see cref="ISecretsConfigurationBuilder"/>.</returns>
        public static ISecretsConfigurationBuilder AddAwsSecret(this ISecretsConfigurationBuilder builder, string key, string awsSecretName, string awsSecretKey = null, IAmazonSecretsManager secretsManager = null) => 
            builder.AddSecret(new AwsSecret(key, awsSecretName, awsSecretKey, secretsManager));
    }
}
