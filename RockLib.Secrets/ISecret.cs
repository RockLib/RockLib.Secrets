namespace RockLib.Secrets
{
    /// <summary>
    /// Defines a secret.
    /// </summary>
    public interface ISecret
    {
        /// <summary>
        /// Gets the key of the secret.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        string GetValue();
    }
}
