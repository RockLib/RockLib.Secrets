using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class SecretsConfigurationSourceTests
    {
        [Fact]
        public void DisableReloadMethod()
        {
            var source = new SecretsConfigurationSource
            {
                ReloadMilliseconds = 1000
            };

            source.DisableReload();

            source.ReloadMilliseconds.Should().Be(Timeout.Infinite);
        }

        [Fact]
        public void BuildMethod()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key2", "value2").Object;
            
            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["RockLib.Secrets:Type"] = typeof(CustomSecret).AssemblyQualifiedName!
                });

            var provider = (SecretsConfigurationProvider)source.Build(builder);

            provider.Source.Should().BeSameAs(source);

            provider.Secrets.Should().NotBeSameAs(source.Secrets);
            provider.Secrets.Should().BeEquivalentTo(source.Secrets);

            provider.Secrets[0].Should().BeSameAs(secret1);
            provider.Secrets[1].Should().BeSameAs(secret2);
            provider.Secrets[2].Should().BeOfType<CustomSecret>();
        }

        [Fact]
        public void BuildMethodWhenOnSecretExceptionIsNull()
        {
            Action<SecretExceptionContext> onSecretException = context => { };

            var source = new SecretsConfigurationSource
            {
                Secrets = { MockSecret.Get("key", "value").Object }
            };

            var builder = new ConfigurationBuilder();
            builder.SetSecretExceptionHandler(onSecretException);

            var provider = source.Build(builder);

            var secretsConfigurationProvider = provider.Should().BeOfType<SecretsConfigurationProvider>().Subject;
            secretsConfigurationProvider.Source.Should().BeSameAs(source);
            source.OnSecretException.Should().BeSameAs(onSecretException);
        }

        [Fact]
        public void BuildMethodAddingSecretsOnFirstCall()
        {
            var secret = MockSecret.Get("key", "value").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret }
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["RockLib.Secrets:Type"] = typeof(CustomSecret).AssemblyQualifiedName!
                });

            // Before building, source only contains the single secret that was added directly to it.
            source.Secrets.Should().ContainSingle(s => ReferenceEquals(s, secret));

            source.Build(builder);

            // After building the first time, the secret defined in configuration has been added to the source.
            source.Secrets.Should().HaveCount(2);
            source.Secrets[0].Should().BeSameAs(secret);
            source.Secrets[1].Should().BeOfType<CustomSecret>();

            source.Build(builder);

            // After building a second time, source hasn't changed.
            source.Secrets.Should().HaveCount(2);
            source.Secrets[0].Should().BeSameAs(secret);
            source.Secrets[1].Should().BeOfType<CustomSecret>();
        }

#pragma warning disable CA1812
        private sealed class CustomSecret : ISecret
#pragma warning restore CA1812
        {
            public string ConfigurationKey => "CustomSecret.Key";

            public string GetValue() => "CustomSecret.GetValue()";
        }
    }
}
