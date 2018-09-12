using System.Collections.Generic;
using System.Linq;

namespace RockLib.Secrets
{
    public class CompositeSecretsProvider : ISecretsProvider
    {
        private readonly IReadOnlyCollection<ISecretsProvider> _secretsProviders;

        public CompositeSecretsProvider(IReadOnlyCollection<ISecretsProvider> secretsProviders) =>
            _secretsProviders = secretsProviders;

        public IReadOnlyList<ISecret> GetSecrets() =>
            _secretsProviders.SelectMany(s => s.GetSecrets()).ToList();
    }
}
