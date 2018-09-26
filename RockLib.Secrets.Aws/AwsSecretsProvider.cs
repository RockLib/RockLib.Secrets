using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.SecretsManager;

namespace RockLib.Secrets.Aws
{
    public class AwsSecretsProvider : ISecretsProvider
    {
        public AwsSecretsProvider(IEnumerable<AwsSecretDefinition> secrets)
            : this(new AmazonSecretsManagerClient(), secrets)
        {
        }

        public AwsSecretsProvider(string region, IEnumerable<AwsSecretDefinition> secrets)
            : this(new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region)), secrets)
        {
        }

        public AwsSecretsProvider(IAmazonSecretsManager secretsClient, IEnumerable<AwsSecretDefinition> secrets)
            : this(secretsClient, secrets.SelectMany(s => s.Keys.Select(k => new AwsSecret(k.Key, k.AwsSecretKey, s.AwsSecretId, secretsClient))).ToList())
        {
        }

        public AwsSecretsProvider(IAmazonSecretsManager secretsClient, IReadOnlyList<AwsSecret> secrets)
        {
            SecretsClient = secretsClient;
            Secrets = secrets;
        }

        public IAmazonSecretsManager SecretsClient { get; }
        public IReadOnlyList<AwsSecret> Secrets { get; }
        IReadOnlyList<ISecret> ISecretsProvider.Secrets => Secrets;
    }
}
