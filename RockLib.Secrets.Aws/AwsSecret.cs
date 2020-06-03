﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json.Linq;
using RockLib.Configuration.ObjectFactory;

namespace RockLib.Secrets.Aws
{
    /// <summary>
    /// An implementation of the <see cref="ISecret"/> interface for use with AWS.
    /// </summary>
    public class AwsSecret : ISecret
    {
        private static readonly Lazy<IAmazonSecretsManager> _defaultSecretsManager = new Lazy<IAmazonSecretsManager>(() => new AmazonSecretsManagerClient());

        private readonly string _exceptionMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for plaintext secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="secretsManager">The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.</param>
        public AwsSecret(string key, string awsSecretName,
            [DefaultType(typeof(AmazonSecretsManagerClient))]IAmazonSecretsManager secretsManager = null)
            : this(key, awsSecretName, null, secretsManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for key/value secrets.
        /// </summary>
        /// <param name="key">The key used to retrieve the secret from the provider.</param>
        /// <param name="awsSecretName">The name of the secret in AWS.</param>
        /// <param name="awsSecretKey">The key of the secret in AWS.</param>
        /// <param name="secretsManager">The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.</param>
        public AwsSecret(string key, string awsSecretName, string awsSecretKey,
            [DefaultType(typeof(AmazonSecretsManagerClient))]IAmazonSecretsManager secretsManager = null)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            AwsSecretName = awsSecretName ?? throw new ArgumentNullException(nameof(awsSecretName));
            AwsSecretKey = awsSecretKey;
            SecretsManager = secretsManager ?? _defaultSecretsManager.Value;

            if (awsSecretKey is null)
                _exceptionMessage = $"No secret was found with the AwsSecretName '{AwsSecretName}' for the key '{Key}'";
            else
                _exceptionMessage = $"No secret was found with the AwsSecretName '{AwsSecretName}' and AwsSecretKey '{AwsSecretKey}' for the key '{Key}'";
        }

        /// <summary>
        /// Gets the key of the secret.
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
        public IAmazonSecretsManager SecretsManager { get; }

        /// <summary>
        /// Gets the value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        public string GetValue()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = AwsSecretName
            };

            // NOTE: Returns an async calls value safely.
            var response = Sync.OverAsync(() => SecretsManager.GetSecretValueAsync(request));

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
