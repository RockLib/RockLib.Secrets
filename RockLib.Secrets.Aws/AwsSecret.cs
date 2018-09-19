using System;
using System.Collections.Generic;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace RockLib.Secrets.Aws
{
    public class AwsSecret : ISecret
    {
        private readonly string _exceptionMessage;

        public AwsSecret(string key, string awsSecretId, IAmazonSecretsManager secretsClient)
        {
            Key = key;
            AwsSecretId = awsSecretId;
            SecretsClient = secretsClient;

            _exceptionMessage = $"No secret was found with the AwsSecretId '{AwsSecretId}' for the key '{Key}'";
        }

        public string Key { get; }
        public string AwsSecretId { get; }
        public IAmazonSecretsManager SecretsClient { get; }

        public string GetValue()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = AwsSecretId
            };

            // NOTE: Returns an async calls value safely.
            var response = Sync.OverAsync(() => SecretsClient.GetSecretValueAsync(request));

            if (response == null)
                throw new KeyNotFoundException(_exceptionMessage);

            if (response.SecretString != null)
                return response.SecretString;

            if (response.SecretBinary != null)
                return Convert.ToBase64String(response.SecretBinary.ToArray());

            throw new KeyNotFoundException(_exceptionMessage);
        }
    }
}
