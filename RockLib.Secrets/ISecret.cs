namespace RockLib.Secrets
{
    /// <summary>
    /// Defines a secret.
    /// </summary>
    public interface ISecret
    {
        /// <summary>
        /// Gets the configuration key for the secret.
        /// </summary>
        string ConfigurationKey { get; }

        /// <summary>
        /// Gets the value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        string GetValue();
    }
}
