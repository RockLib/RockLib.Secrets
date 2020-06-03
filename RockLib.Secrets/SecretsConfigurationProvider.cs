using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        private readonly IReadOnlyList<ISecret> _secrets;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationProvider"/> class.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public SecretsConfigurationProvider(SecretsConfigurationSource source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (source.Secrets.Any(s => s is null))
                throw new ArgumentException("Secrets cannot contain any null items.", nameof(source));
            if (source.Secrets.Any(s => s.Key is null))
                throw new ArgumentException("Secrets cannot contain any items with a null Key.", nameof(source));
            if (source.Secrets.Select(s => s.Key).Distinct(StringComparer.OrdinalIgnoreCase).Count() != source.Secrets.Count)
                throw new ArgumentException("Secrets cannot contain any items with duplicate Keys.", nameof(source));

            Source = source;
            _timer = new Timer(_ => Load());
            _secrets = source.Secrets.ToArray();
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public SecretsConfigurationSource Source { get; }

        /// <summary>
        /// Loads data from the secrets provider.
        /// </summary>
        public override void Load()
        {
            try
            {
                var secretValues = _secrets.Select(secret =>
                {
                    try
                    {
                        return new { secret.Key, Value = secret.GetValue() };
                    }
                    catch (Exception ex)
                    {
                        Source.OnSecretException?.Invoke(new SecretExceptionContext(this, ex));
                        return null;
                    }
                }).Where(s => s != null);

                var secretChanged = false;

                if (Data.Count == 0)
                {
                    foreach (var secret in secretValues)
                        Data.Add(secret.Key, secret.Value);
                }
                else
                {
                    foreach (var secret in secretValues)
                    {
                        if (Data[secret.Key] != secret.Value)
                        {
                            secretChanged = true;
                            Data[secret.Key] = secret.Value;
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
