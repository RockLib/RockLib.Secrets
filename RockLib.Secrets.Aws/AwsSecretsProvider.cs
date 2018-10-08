using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.SecretsManager;

namespace RockLib.Secrets.Aws
{
    /// <summary>
    /// An implementation of the <see cref="ISecretsProvider"/> interface that
    /// pull its secrets from AWS Secrets Manager.
    /// </summary>
    public class AwsSecretsProvider : ISecretsProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsProvider"/> class.
        /// </summary>
        /// <param name="secrets">A list of <see cref="AwsSecretDefinition"/>.</param>
        public AwsSecretsProvider(IEnumerable<AwsSecretDefinition> secrets)
            : this(new AmazonSecretsManagerClient(), secrets)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsProvider"/> class.
        /// </summary>
        /// <param name="region">The AWS region.</param>
        /// <param name="secrets">A list of <see cref="AwsSecretDefinition"/> to define the secrets.</param>
        public AwsSecretsProvider(string region, IEnumerable<AwsSecretDefinition> secrets)
            : this(new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region)), secrets)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsProvider"/> class.
        /// </summary>
        /// <param name="secretsClient">The <see cref="IAmazonSecretsManager"/> used for routing calls to AWS.</param>
        /// <param name="secrets">A list of <see cref="AwsSecretDefinition"/> to define the secrets.</param>
        public AwsSecretsProvider(IAmazonSecretsManager secretsClient, IEnumerable<AwsSecretDefinition> secrets)
            : this(secretsClient, secrets.Select(s => new AwsSecret(s.Key, s.AwsSecretName, s.AwsSecretKey, secretsClient)).ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsProvider"/> class.
        /// </summary>
        /// <param name="secretsClient">The <see cref="IAmazonSecretsManager"/> used for routing calls to AWS.</param>
        /// <param name="secrets">A list of <see cref="AwsSecret"/> to define the secrets.</param>
        public AwsSecretsProvider(IAmazonSecretsManager secretsClient, IReadOnlyList<AwsSecret> secrets)
        {
            SecretsClient = secretsClient;
            Secrets = secrets;
        }

        /// <summary>
        /// Gets the <see cref="IAmazonSecretsManager"/> used for routing calls to AWS.
        /// </summary>
        public IAmazonSecretsManager SecretsClient { get; }

        /// <summary>
        /// Gets the list of <see cref="AwsSecret"/> that define the secrets.
        /// </summary>
        public IReadOnlyList<AwsSecret> Secrets { get; }

        IReadOnlyList<ISecret> ISecretsProvider.Secrets => Secrets;
    }
}
