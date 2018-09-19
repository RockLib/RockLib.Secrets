namespace RockLib.Secrets.Aws
{
    public class AwsSecretDefinition
    {
        public AwsSecretDefinition(string key, string awsSecretId)
        {
            Key = key;
            AwsSecretId = awsSecretId;
        }

        public string Key { get; }
        public string AwsSecretId { get; }
    }
}