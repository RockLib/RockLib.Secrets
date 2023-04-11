using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly string _exceptionMessageFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for key/value secrets.
        /// </summary>
        /// <param name="configurationKey">The configuration key for the secret.</param>
        /// <param name="secretId">The Amazon Resource Name (ARN) or the friendly name of the secret.</param>
        /// <param name="secretKey">The key of the secret in AWS.</param>
        /// <param name="secretsManager">
        /// The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS. If <c>null</c>, an instance of <see cref="AmazonSecretsManagerClient"/> will be used.
        /// </param>
        public AwsSecret(string configurationKey, string secretId, string? secretKey = null, 
            IAmazonSecretsManager? secretsManager = null)
        {
            ConfigurationKey = configurationKey ?? throw new ArgumentNullException(nameof(configurationKey));
            SecretId = secretId ?? throw new ArgumentNullException(nameof(secretId));
            SecretKey = secretKey;
            SecretsManager = secretsManager;

            if (secretKey is null)
            {
                _exceptionMessageFormat = $"No secret was found with the AwsSecretName '{SecretId}' for the key '{ConfigurationKey}': {{0}}";
            }
            else
            {
                _exceptionMessageFormat = $"No secret was found with the AwsSecretName '{SecretId}' and AwsSecretKey '{SecretKey}' for the key '{ConfigurationKey}': {{0}}";
            }
        }
        /// <summary>
        /// Gets the configuration key for the secret.
        /// </summary>
        public string ConfigurationKey { get; }

        /// <summary>
        /// Gets the Amazon Resource Name (ARN) or the friendly name of the secret.
        /// </summary>
        public string SecretId { get; }

        /// <summary>
        /// Gets the "Secret Key" of the secret.
        /// </summary>
        public string? SecretKey { get; }

        /// <summary>
        /// Gets the <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS.
        /// </summary>
        public IAmazonSecretsManager? SecretsManager { get; private set; }

        /// <summary>
        /// Gets the value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        public string GetValue()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = SecretId
            };

            // Set the manager to the AWS one if it wasn't provided.
            SecretsManager ??= new AmazonSecretsManagerClient();

            // NOTE: This is NOT ideal. We should be awaiting the call.
            // But ISecret only has a sync version for GetValue().
            // Furthermore, providing an async version (GetValueAsync()) doesn't help,
            // because configuration is synchronous.
            // This issue, if resolved, would fix this: https://github.com/dotnet/runtime/issues/36018
            // But it's still open :(. So we have to block the call here for now.
            var response = SecretsManager.GetSecretValueAsync(request).GetAwaiter().GetResult();

            if (response is null)
            {
                throw new KeyNotFoundException(
                    string.Format(CultureInfo.InvariantCulture, 
                        _exceptionMessageFormat, "Response was null."));
            }

            if (response.SecretString is not null)
            {
                if (SecretKey is not null)
                {
                    var secret = JObject.Parse(response.SecretString)[SecretKey];

                    if (secret is not null)
                    {
                        return secret.ToString();
                    }

                    throw new KeyNotFoundException(
                        string.Format(CultureInfo.InvariantCulture, 
                            _exceptionMessageFormat, $"Response did not contain item with the name '{SecretKey}'."));
                }

                return response.SecretString;
            }

            if (response.SecretBinary is not null)
            {
                return Convert.ToBase64String(response.SecretBinary.ToArray());
            }

            throw new KeyNotFoundException(
                string.Format(CultureInfo.InvariantCulture, 
                    _exceptionMessageFormat, "Response did not contain a value for SecretString or SecretBinary."));
        }
    }
}