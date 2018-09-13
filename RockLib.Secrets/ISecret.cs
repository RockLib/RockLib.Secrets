namespace RockLib.Secrets
{
    /// <summary>
    /// Defines a secret.
    /// </summary>
    public interface ISecret
    {
        /// <summary>
        /// The identifier of the secret.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The value of the secret.
        /// </summary>
        /// <returns>The secret value.</returns>
        string GetValue();
    }
}
