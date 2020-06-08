using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace RockLib.Secrets
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationProvider"/> backed by a collection of secrets.
    /// </summary>
    public class SecretsConfigurationProvider : ConfigurationProvider
    {
        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationProvider"/> class.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public SecretsConfigurationProvider(SecretsConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Secrets = new ReadOnlyCollection<ISecret>(source.Secrets.ToArray());
            _timer = new Timer(_ => Load());

            if (!Secrets.Any())
                throw new ArgumentException("Source must contain at least one secret.", nameof(source));
            if (Secrets.Any(s => s is null))
                throw new ArgumentException("Source cannot contain any null secrets.", nameof(source));
            if (Secrets.Any(s => s.ConfigurationKey is null))
                throw new ArgumentException("Source cannot contain any secrets with a null Key.", nameof(source));
            if (Secrets.Select(s => s.ConfigurationKey).Distinct(StringComparer.OrdinalIgnoreCase).Count() != Secrets.Count)
                throw new ArgumentException("Source cannot contain any secrets with duplicate Keys.", nameof(source));
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public SecretsConfigurationSource Source { get; }

        /// <summary>
        /// The secrets that define this provider.
        /// </summary>
        public IReadOnlyList<ISecret> Secrets { get; }

        /// <summary>
        /// Loads data from <see cref="Secrets"/>.
        /// </summary>
        public override void Load()
        {
            try
            {
                var secrets = Secrets.Select(secret =>
                {
                    try
                    {
                        return new { secret.ConfigurationKey, Value = secret.GetValue() };
                    }
                    catch (Exception ex)
                    {
                        Source.OnSecretException?.Invoke(new SecretExceptionContext(this, secret, ex));
                        return new { secret.ConfigurationKey, Value = (string)null };
                    }
                });

                var secretChanged = false;

                if (Data.Count == 0)
                {
                    foreach (var secret in secrets)
                        Data.Add(secret.ConfigurationKey, secret.Value);
                }
                else
                {
                    foreach (var secret in secrets)
                    {
                        if (secret.Value != null && Data[secret.ConfigurationKey] != secret.Value)
                        {
                            secretChanged = true;
                            Data[secret.ConfigurationKey] = secret.Value;
                        }
                    }
                }

                if (secretChanged)
                    OnReload();
            }
            finally
            {
                _timer.Change(Source.ReloadMilliseconds, Timeout.Infinite);
            }
        }
    }
}
