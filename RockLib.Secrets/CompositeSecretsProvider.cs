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
        private readonly Lazy<IReadOnlyList<ISecret>> _secrets;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSecretsProvider"/> class.
        /// </summary>
        /// <param name="providers">
        /// The implementations of <see cref="ISecretsProvider"/> that are the source of the
        /// secrets returned by the <see cref="Secrets"/> property.
        /// </param>
        public CompositeSecretsProvider(IReadOnlyCollection<ISecretsProvider> providers)
        {
            Providers = providers ?? throw new ArgumentNullException(nameof(providers));
            _secrets = new Lazy<IReadOnlyList<ISecret>>(() => Providers.SelectMany(p => p.Secrets).ToArray());
        }

        /// <summary>
        /// Gets the implementations of <see cref="ISecretsProvider"/> that are
        /// the source of the secrets returned by the <see cref="Secrets"/> property.
        /// </summary>
        public IReadOnlyCollection<ISecretsProvider> Providers { get; }

        /// <summary>
        /// Gets the secrets for this provider.
        /// </summary>
        /// <returns>A list of secrets for this provider.</returns>
        public IReadOnlyList<ISecret> Secrets => _secrets.Value;
    }
}
