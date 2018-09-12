using System;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    public interface ISecretsProvider
    {
        IReadOnlyList<ISecret> GetSecrets();
    }
}
