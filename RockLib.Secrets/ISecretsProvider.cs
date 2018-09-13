using System;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    /// <summary>
    /// Defines an interface for providing secrets.
    /// </summary>
    public interface ISecretsProvider
    {
        /// <summary>
        /// Gets all available secrets from this provider.
        /// </summary>
        /// <returns>A list of secrets.</returns>
        IReadOnlyList<ISecret> GetSecrets();
    }
}
