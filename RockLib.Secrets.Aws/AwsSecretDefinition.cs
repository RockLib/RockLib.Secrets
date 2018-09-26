using System.Collections.Generic;

namespace RockLib.Secrets.Aws
{
    public class AwsSecretDefinition
    {
        public AwsSecretDefinition(string awsSecretId, IEnumerable<AwsSecretKeyDefinition> keys)
        {
            AwsSecretId = awsSecretId;
            Keys = keys;
        }

        public string AwsSecretId { get; }
        public IEnumerable<AwsSecretKeyDefinition> Keys { get; }
    }
}