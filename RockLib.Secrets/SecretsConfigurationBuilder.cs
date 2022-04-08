using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// A builder object that adds secrets to a secrets source.
    /// </summary>
    public class SecretsConfigurationBuilder : ISecretsConfigurationBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationBuilder"/> class.
        /// </summary>
        /// <param name="source">The secrets configuration source.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is <c>null</c>.
        /// </exception>
        public SecretsConfigurationBuilder(SecretsConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// The source backing the builder.
        /// </summary>
        public SecretsConfigurationSource Source { get; }

        /// <summary>
        /// Adds a secret to the secrets source.
        /// </summary>
        /// <param name="secret">The <see cref="ISecret"/>.</param>
        /// <returns>The <see cref="ISecretsConfigurationBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="secret"/> is <c>null</c>.
        /// </exception>
        public ISecretsConfigurationBuilder AddSecret(ISecret secret)
        {
            Source.Secrets.Add(secret ?? throw new ArgumentNullException(nameof(secret)));
            return this;
        }
    }
}