using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Secrets
{
    /// <summary>
    /// An implementation of <see cref="ISecretsProvider"/> that gets its
    /// secrets from multiple implementations of <see cref="ISecretsProvider"/>.
    /// </summary>
    public class CompositeSecretsProvider : ISecretsProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSecretsProvider"/> class.
        /// </summary>
        /// <param name="secretsProviders"></param>
        public CompositeSecretsProvider(IReadOnlyCollection<ISecretsProvider> secretsProviders) =>
            Providers = secretsProviders ?? throw new ArgumentNullException(nameof(secretsProviders));

        /// <summary>
        /// Gets the implementations of <see cref="ISecretsProvider"/> that are
        /// the source of the secrets returned by the <see cref="GetSecrets"/> method.
        /// the secrets
        /// </summary>
        public IReadOnlyCollection<ISecretsProvider> Providers { get; }

        /// <summary>
        /// Gets all available secrets from all providers in <see cref="Providers"/>.
        /// </summary>
        /// <returns>A list of secrets.</returns>
        public IReadOnlyList<ISecret> GetSecrets() =>
            Providers.SelectMany(s => s.GetSecrets()).ToList();
    }
}
