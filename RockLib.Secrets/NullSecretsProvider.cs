using System.Collections.Generic;

namespace RockLib.Secrets
{
    public class NullSecretsProvider : ISecretsProvider
    {
        private static readonly ISecret[] _empty = new ISecret[0];
        public IReadOnlyList<ISecret> GetSecrets() => _empty;
    }
}
