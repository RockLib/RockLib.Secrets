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
        /// Gets the secrets for this provider.
        /// </summary>
        /// <returns>A list of secrets for this provider.</returns>
        IReadOnlyList<ISecret> Secrets { get; }
    }
}
