namespace RockLib.Secrets.Aws
{
    public class AwsSecretDefinition
    {
        public AwsSecretDefinition(string key, string awsSecretId, string awsSecretKey)
        {
            Key = key;
            AwsSecretId = awsSecretId;
            AwsSecretKey = awsSecretKey;
        }

        public string Key { get; }
        public string AwsSecretId { get; }
        public string AwsSecretKey { get; }
    }
}