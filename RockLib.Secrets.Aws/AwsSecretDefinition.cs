namespace RockLib.Secrets.Aws
{
    /// <summary>
    /// A class that defines the necessary information for retrieving an AWS secret.
    /// </summary>
    public class AwsSecretDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretDefinition"/> class for plaintext secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        public AwsSecretDefinition(string key, string awsSecretName)
            : this(key, awsSecretName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretDefinition"/> class for key/value secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="awsSecretKey">The key of the secret in AWS.</param>
        public AwsSecretDefinition(string key, string awsSecretName, string awsSecretKey)
        {
            Key = key;
            AwsSecretName = awsSecretName;
            AwsSecretKey = awsSecretKey;
        }

        /// <summary>
        /// Gets the key used to retrieve the secret from the provider.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the name of the secret in AWS.
        /// </summary>
        public string AwsSecretName { get; }

        /// <summary>
        /// Gets the key of the secret in AWS.
        /// </summary>
        public string AwsSecretKey { get; }
    }
}