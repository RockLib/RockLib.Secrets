using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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
        private static IAmazonSecretsManager? _defaultSecretsManager;
        private readonly string _exceptionMessageFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecret"/> class for key/value secrets.
        /// </summary>
        /// <param name="configurationKey">The configuration key for the secret.</param>
        /// <param name="secretId">The Amazon Resource Name (ARN) or the friendly name of the secret.</param>
        /// <param name="secretKey">The key of the secret in AWS.</param>
        /// <param name="secretsManager">
        /// The <see cref="IAmazonSecretsManager"/> client used for routing calls to AWS. If <see langword="null"/>,
        /// then <see cref="AwsSecret.DefaultSecretsManager"/> is used instead.
        /// </param>
        public AwsSecret(string configurationKey, string secretId, string? secretKey = null,
            [DefaultType(typeof(AmazonSecretsManagerClient))]IAmazonSecretsManager? secretsManager = null)
        {
            ConfigurationKey = configurationKey ?? throw new ArgumentNullException(nameof(configurationKey));
            SecretId = secretId ?? throw new ArgumentNullException(nameof(secretId));
            SecretKey = secretKey;
            SecretsManager = secretsManager ?? DefaultSecretsManager;

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
        /// Gets or sets the instance of <see cref="IAmazonSecretsManager"/> to be used by the
        /// <see cref="AwsSecret"/> class when a secrets manager is not provided to its constructor.
        /// </summary>
        public static IAmazonSecretsManager DefaultSecretsManager
        {
            get => _defaultSecretsManager ??= new AmazonSecretsManagerClient();
            set => _defaultSecretsManager = value ?? throw new ArgumentNullException(nameof(value));
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
        public IAmazonSecretsManager SecretsManager { get; }

        /// <summary>
        /// Gets the value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        public async Task<string> GetValueAsync()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = SecretId
            };

            // NOTE: Returns an async calls value safely.
            var response = await SecretsManager.GetSecretValueAsync(request).ConfigureAwait(false);

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
                return Convert.ToBase64String(response.SecretBinary.ToArray());

            throw new KeyNotFoundException(
                string.Format(CultureInfo.InvariantCulture, 
                    _exceptionMessageFormat, "Response did not contain a value for SecretString or SecretBinary."));
        }
    }
}