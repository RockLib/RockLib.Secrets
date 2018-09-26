using System;
using System.Collections.Generic;
using System.Text;

namespace RockLib.Secrets.Aws
{
    public class AwsSecretKeyDefinition
    {
        public AwsSecretKeyDefinition(string key, string awsSecretKey)
        {
            Key = key;
            AwsSecretKey = awsSecretKey;
        }

        public string Key { get; }
        public string AwsSecretKey { get; }
    }
}
