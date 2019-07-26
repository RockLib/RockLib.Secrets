using System;
using System.Collections.Generic;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json.Linq;

namespace RockLib.Secrets.Aws
{
    /// <summary>
    /// An implementation of the <see cref="ISecret"/> interface for use with AWS.
    /// </summary>
    public class AwsSecret : ISecret
    {
        private readonly string _exceptionMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for plaintext secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="secretsClient">The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.</param>
        public AwsSecret(string key, string awsSecretName, IAmazonSecretsManager secretsClient)
            : this(key, awsSecretName, null, secretsClient)
        {
            _exceptionMessage = $"No secret was found with the AwsSecretName '{AwsSecretName}' for the key '{Key}'";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for key/value secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="awsSecretKey">The key of the secret in AWS.</param>
        /// <param name="secretsClient">The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.</param>
        public AwsSecret(string key, string awsSecretName, string awsSecretKey, IAmazonSecretsManager secretsClient)
        {
            Key = key;
            AwsSecretName = awsSecretName;
            AwsSecretKey = awsSecretKey;
            SecretsClient = secretsClient;

            _exceptionMessage = $"No secret was found with the AwsSecretName '{AwsSecretName}' and AwsSecretKey '{AwsSecretKey}' for the key '{Key}'";
        }

        /// <summary>
        /// Gets the identifier of the secret.
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

        /// <summary>
        /// Gets the <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.
        /// </summary>
        public IAmazonSecretsManager SecretsClient { get; }

        /// <summary>
        /// Returns the value of the secret.
        /// </summary>
        /// <returns>The string value of the secret.</returns>
        public string GetValue()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = AwsSecretName
            };

            // NOTE: Returns an async calls value safely.
            var response = Sync.OverAsync(() => SecretsClient.GetSecretValueAsync(request));

            if (response == null)
                throw new KeyNotFoundException(_exceptionMessage);

            if (response.SecretString != null)
            {
                if (AwsSecretKey != null)
                {
                    var secret = JObject.Parse(response.SecretString)[AwsSecretKey];
                    if (secret != null)
                        return secret.ToString();

                    throw new KeyNotFoundException(_exceptionMessage);
                }

                return response.SecretString;
            }

            if (response.SecretBinary != null)
                return Convert.ToBase64String(response.SecretBinary.ToArray());

            throw new KeyNotFoundException(_exceptionMessage);
        }
    }
}
