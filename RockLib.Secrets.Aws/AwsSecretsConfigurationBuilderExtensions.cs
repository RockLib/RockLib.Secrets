using Amazon.SecretsManager;
using System;

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
        /// <param name="configurationKey">The configuration key for the secret.</param>
        /// <param name="secretId">The Amazon Resource Name (ARN) or the friendly name of the secret.</param>
        /// <param name="secretKey">The key of the secret in AWS.</param>
        /// <param name="secretsManager">
        /// The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS. If <see langword="null"/>,
        /// then <see cref="AwsSecret.DefaultSecretsManager"/> is used instead.
        /// </param>
        /// <returns>The same <see cref="ISecretsConfigurationBuilder"/>.</returns>
        public static ISecretsConfigurationBuilder AddAwsSecret(this ISecretsConfigurationBuilder builder,
            string configurationKey, string secretId, string secretKey = null, IAmazonSecretsManager secretsManager = null)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            return builder.AddSecret(new AwsSecret(configurationKey, secretId, secretKey, secretsManager));
        }
    }
}
