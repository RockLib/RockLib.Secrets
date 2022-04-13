using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using RockLib.Secrets.Aws;
using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// Extension methods for configuration builders related to AWS.
    /// </summary>
    public static class AwsConfigurationBuilderExtensions
    {
        /// <summary>
        /// Sets the instance of <see cref="IAmazonSecretsManager"/> to be used by the <see cref="AwsSecret"/>
        /// class when a secrets manager is not provided to its constructor.
        /// </summary>
        /// <remarks>
        /// This method does not modify the <see cref="IConfigurationBuilder"/>; it sets the value of the
        /// static <see cref="AwsSecret.DefaultSecretsManager"/> property.
        /// </remarks>
        /// <param name="builder">A <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="secretsManager"></param>
        /// <returns>The same <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder SetAmazonSecretsManager(this IConfigurationBuilder builder,
            IAmazonSecretsManager secretsManager)
        {
            AwsSecret.DefaultSecretsManager = secretsManager ?? throw new ArgumentNullException(nameof(secretsManager));
            return builder;
        }
    }
}