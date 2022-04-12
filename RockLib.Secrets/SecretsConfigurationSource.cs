using Microsoft.Extensions.Configuration;
using RockLib.Configuration;
using RockLib.Configuration.ObjectFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RockLib.Secrets
{
    /// <summary>
    /// Represents a source of secrets as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class SecretsConfigurationSource : IConfigurationSource
    {
        /// <summary>The default value of the <see cref="ReloadMilliseconds"/> property, 5 minutes.</summary>
        public const int DefaultReloadMilliseconds = 5 * 60 * 1000;

        private int _reloadMilliseconds = DefaultReloadMilliseconds;
        private bool _alreadyBuilt;

        /// <summary>
        /// Gets the list of <see cref="ISecret"/> 
        /// </summary>
        public IList<ISecret> Secrets { get; } = new List<ISecret>();

        /// <summary>
        /// Will be called if an uncaught exception occurs when calling <see cref="ISecret.GetValue"/>
        /// from the <see cref="SecretsConfigurationProvider.Load"/> method.
        /// </summary>
        public Action<SecretExceptionContext>? OnSecretException { get; set; }

        /// <summary>
        /// The number of milliseconds to wait before reloading secrets. Specify <see cref="Timeout.Infinite"/>
        /// to disable reloading.
        /// </summary>
        public int ReloadMilliseconds
        {
            get => _reloadMilliseconds;
            set
            {
                if (value < Timeout.Infinite)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be either non-negative or -1.");
                }
                _reloadMilliseconds = value;
            }
        }

        /// <summary>
        /// Disables periodic reloading.
        /// </summary>
        public void DisableReload() => ReloadMilliseconds = Timeout.Infinite;

        /// <summary>
        /// Builds the <see cref="SecretsConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An instance of <see cref="SecretsConfigurationProvider"/>.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!_alreadyBuilt)
            {
                _alreadyBuilt = true;
                foreach (var secretFromConfiguration in CreateSecretsFromConfiguration(builder))
                {
                    Secrets.Add(secretFromConfiguration);
                }
            }

            OnSecretException ??= builder.GetSecretExceptionHandler();

            return new SecretsConfigurationProvider(this);
        }

        private IEnumerable<ISecret> CreateSecretsFromConfiguration(IConfigurationBuilder builder)
        {
            // Make a copy of the builder, excluding this SecretsConfigurationSource.
            // Otherwise there will be infinite recursion when building the builder.
            var builderCopy = new ConfigurationBuilder();

            foreach (var source in builder.Sources.Where(s => !ReferenceEquals(this, s)))
            {
                builderCopy.Add(source);
            }

            foreach (var property in builder.Properties)
            {
                builderCopy.Properties.Add(property.Key, property.Value);
            }

            var configuration = builderCopy.Build();
            
            return configuration.GetCompositeSection("RockLib_Secrets", "RockLib.Secrets")
                .Create<List<ISecret>>()!;
        }
    }
}
