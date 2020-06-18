namespace RockLib.Secrets
{
    /// <summary>
    /// Defines a builder for adding secrets.
    /// </summary>
    public interface ISecretsConfigurationBuilder
    {
        /// <summary>
        /// Adds a secret to the builder.
        /// </summary>
        /// <param name="secret">The <see cref="ISecret"/>.</param>
        /// <returns>The <see cref="ISecretsConfigurationBuilder"/>.</returns>
        ISecretsConfigurationBuilder AddSecret(ISecret secret);
    }
}
