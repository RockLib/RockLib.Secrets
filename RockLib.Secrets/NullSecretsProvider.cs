using System.Collections.Generic;

namespace RockLib.Secrets
{
    /// <summary>
    /// An implementation of <see cref="ISecretsProvider"/> that returns an
    /// empty list of secrets.
    /// </summary>
    public class NullSecretsProvider : ISecretsProvider
    {
        private static readonly ISecret[] _empty = new ISecret[0];

        /// <summary>
        /// Returns an empty list of secrets.
        /// </summary>
        /// <returns>An empty list of secrets.</returns>
        public IReadOnlyList<ISecret> Secrets => _empty;
    }
}
