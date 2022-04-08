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
    public class SecretsConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly Timer _timer;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretsConfigurationProvider"/> class.
        /// </summary>
        /// <param name="source">The source settings.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the secrets collection has any unexpected issues.
        /// </exception>
        public SecretsConfigurationProvider(SecretsConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Secrets = new ReadOnlyCollection<ISecret>(source.Secrets.ToArray());
            _timer = new Timer(_ => Load());

            if (!Secrets.Any())
            {
                throw new ArgumentException("The SecretsConfigurationSource must contain at least one secret.", nameof(source));
            }
            if (Secrets.Any(s => s is null))
            {
                throw new ArgumentException("The SecretsConfigurationSource cannot contain any null secrets.", nameof(source));
            }
            if (Secrets.Any(s => s.ConfigurationKey is null))
            {
                throw new ArgumentException("The SecretsConfigurationSource cannot contain any secrets with a null Key.", nameof(source));
            }
            if (Secrets.Select(s => s.ConfigurationKey).Distinct(StringComparer.OrdinalIgnoreCase).Count() != Secrets.Count)
            {
                throw new ArgumentException("The SecretsConfigurationSource cannot contain any secrets with duplicate Keys.", nameof(source));
            }
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
                    // Would love to catch a specific exception, but...
                    // we don't know what the implementation of ISecret.GetValue()
                    // will do here, and this code has already been published, so...
                    // we have to keep this as a general Exception.
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        Source.OnSecretException?.Invoke(new SecretExceptionContext(this, secret, ex));
                        return new { secret.ConfigurationKey, Value = (string)null! };
                    }
                });

                var secretChanged = false;

                if (Data.Count == 0)
                {
                    foreach (var secret in secrets)
                    {
                        Data.Add(secret.ConfigurationKey, secret.Value);
                    }
                }
                else
                {
                    foreach (var secret in secrets)
                    {
                        if (secret.Value is not null && Data[secret.ConfigurationKey] != secret.Value)
                        {
                            secretChanged = true;
                            Data[secret.ConfigurationKey] = secret.Value;
                        }
                    }
                }

                if (secretChanged)
                {
                    OnReload();
                }
            }
            finally
            {
                _timer.Change(Source.ReloadMilliseconds, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        ~SecretsConfigurationProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        /// <param name="dispose">Specifies if this is a managed or unmanaged disposal</param>
        protected virtual void Dispose(bool dispose)
        {
            if(!_disposed)
            {
                if(dispose)
                {
                    _timer.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
