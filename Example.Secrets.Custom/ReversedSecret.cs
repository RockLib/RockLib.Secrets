using RockLib.Secrets;
using System.Linq;

namespace Example.Secrets.Custom
{
    public class ReversedSecret : ISecret
    {
        private readonly string _secret;

        public ReversedSecret(string key, string secret)
        {
            ConfigurationKey = key;
            _secret = secret;
        }

        public string ConfigurationKey { get; }

        public string GetValue()
        {
            return new string(_secret.ToCharArray().Reverse().ToArray());
        }
    }
}
