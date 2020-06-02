using Microsoft.Extensions.Configuration;
using RockLib.Configuration;
using RockLib.Configuration.ObjectFactory;
using System;
using System.Collections.Generic;

namespace RockLib.Secrets
{
    /// <summary>
    /// Extension methods for adding <see cref="SecretsConfigurationProvider"/>.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// The key to the <see cref="IConfigurationBuilder.Properties"/> where the
        /// default <see cref="ISecretsProvider"/> is stored.
        /// </summary>
        public const string SecretsProviderKey = "RockLib.SecretsProvider";

        /// <summary>
        /// Adds a secrets configuration source to the <paramref name="builder"/>.
        /// <para>
        /// The underlying implementation of <see cref="ISecretsProvider"/> is created by building
        /// the current value of the <paramref name="builder"/> parameter and using the value of
        /// its "RockLib_Secrets" / "RockLib.Secrets" configuration section to create the instance.
        /// If no secrets provider is defined in configuration, then the secret provider returned by
        /// the <see cref="GetSecretsProvider"/> extension method is used instead.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder) =>
            builder.AddRockLibSecrets(builder.CreateSecretsProvider());

        /// <summary>
        /// Adds a secrets configuration source to the <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="secretsProvider">
        /// The implementation of <see cref="ISecretsProvider"/> that provides the secrets that are mapped
        /// to configuration settings. If <see langword="null"/>, then the secret provider returned by
        /// the <see cref="GetSecretsProvider"/> extension method is used instead.
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder, ISecretsProvider secretsProvider)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            return builder.AddRockLibSecrets(s =>
            {
                s.SecretsProvider = secretsProvider;
            });
        }

        /// <summary>
        /// Adds a secrets configuration source to the <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">
        /// Configures the secrets configuration source. If this action does not set the
        /// <see cref="SecretsConfigurationSource.SecretsProvider"/> property, then the secrets provider
        /// returned by the <see cref="GetSecretsProvider"/> extension method is used instead.
        /// </param>
        /// <returns>The <see cref="IConfigurationBuilder"/></returns>
        public static IConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder, Action<SecretsConfigurationSource> configureSource)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var source = new SecretsConfigurationSource();
            configureSource?.Invoke(source);
            return builder.Add(source);
        }

        /// <summary>
        /// Sets the default <see cref="ISecretsProvider"/> to be used for SecretsConfigurationProviders.
        /// The secrets provider will be returned by the <see cref="GetSecretsProvider"/> extension method.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="secretsProvider">The default secrets provider instance.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder SetSecretsProvider(this IConfigurationBuilder builder, ISecretsProvider secretsProvider)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Properties[SecretsProviderKey] = secretsProvider ?? throw new ArgumentNullException(nameof(secretsProvider));
            return builder;
        }

        /// <summary>
        /// Gets the default <see cref="ISecretsProvider"/> to be used for SecretsConfigurationProviders
        /// that was set by the <see cref="SetSecretsProvider"/> extension method.
        /// </summary>
        /// <param name="builder">The default <see cref="ISecretsProvider"/>.</param>
        /// <returns>The default <see cref="ISecretsProvider"/>.</returns>
        public static ISecretsProvider GetSecretsProvider(this IConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Properties.TryGetValue(SecretsProviderKey, out var value) && value is ISecretsProvider secretsProvider)
                return secretsProvider;

            return null;
        }

        private static ISecretsProvider CreateSecretsProvider(this IConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var configuration = builder.Build();
            var secretsProviders = configuration.GetCompositeSection("RockLib_Secrets", "RockLib.Secrets")
                .Create<List<ISecretsProvider>>();
            if (secretsProviders.Count == 0)
                return null;
            if (secretsProviders.Count == 1)
                return secretsProviders[0];
            return new CompositeSecretsProvider(secretsProviders);
        }
    }
}
