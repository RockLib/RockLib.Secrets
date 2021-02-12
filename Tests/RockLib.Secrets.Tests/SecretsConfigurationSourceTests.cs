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
        [Fact(DisplayName = "DisableReload method sets ReloadMilliseconds to Timeout.Infinite")]
        public void DisableReloadMethodHappyPath()
        {
            var source = new SecretsConfigurationSource
            {
                ReloadMilliseconds = 1000
            };

            source.DisableReload();

            source.ReloadMilliseconds.Should().Be(Timeout.Infinite);
        }

        [Fact(DisplayName = "Build method returns SecretsConfigurationProvider")]
        public void BuildMethodHappyPath1()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key2", "value2").Object;
            
            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["RockLib.Secrets:Type"] = typeof(CustomSecret).AssemblyQualifiedName
                });

            var provider = (SecretsConfigurationProvider)source.Build(builder);

            provider.Source.Should().BeSameAs(source);

            provider.Secrets.Should().NotBeSameAs(source.Secrets);
            provider.Secrets.Should().BeEquivalentTo(source.Secrets);

            provider.Secrets[0].Should().BeSameAs(secret1);
            provider.Secrets[1].Should().BeSameAs(secret2);
            provider.Secrets[2].Should().BeOfType<CustomSecret>();
        }

        [Fact(DisplayName = "Build method sets OnSecretException property from builder when OnSecretException is null")]
        public void BuildMethodHappyPath2()
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

        [Fact(DisplayName = "Build method only add secrets from configuration on the first call")]
        public void BuildMethodHappyPath3()
        {
            var secret = MockSecret.Get("key", "value").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret }
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["RockLib.Secrets:Type"] = typeof(CustomSecret).AssemblyQualifiedName
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

        private class CustomSecret : ISecret
        {
            public string ConfigurationKey => "CustomSecret.Key";

            public string GetValue() => "CustomSecret.GetValue()";
        }
    }
}
