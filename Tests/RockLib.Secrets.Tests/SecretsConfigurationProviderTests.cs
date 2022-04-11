using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public static class SecretsConfigurationProviderTests
    {
        [Fact]
        public static void Create()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key2", "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            using var provider = new SecretsConfigurationProvider(source);

            provider.Source.Should().BeSameAs(source);

            provider.Secrets.Should().NotBeSameAs(source.Secrets);
            provider.Secrets.Should().BeEquivalentTo(source.Secrets);
        }

        [Fact]
        public static void CreateWithNullSource()
        {
            var act = () => new SecretsConfigurationProvider(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public static void CreateWithNoSecrets()
        {
            var source = new SecretsConfigurationSource();

            var act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource must contain at least one secret.*source*");
        }

        [Fact]
        public static void CreateWithNullValueInSecrets()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            ISecret secret2 = null!;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any null secrets.*source*");
        }

        [Fact]
        public static void CreateWithNullKeyInSecrets()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get(null!, "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any secrets with a null Key.*source*");
        }

        [Fact]
        public static void CreateWithDuplicateKeysInSecrets()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key1", "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any secrets with duplicate Keys.*source*");
        }

        [Fact]
        public static void LoadMethod()
        {
            var secret1 = MockSecret.Get("foo", "abc").Object;
            var secret2 = MockSecret.Get("bar", (string)null!).Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 },
                ReloadMilliseconds = Timeout.Infinite
            };

            using var provider = new SecretsConfigurationProvider(source);

            provider.Load();

            provider.TryGet("foo", out var fooValue).Should().BeTrue();
            fooValue.Should().Be("abc");

            provider.TryGet("bar", out var barValue).Should().BeTrue();
            barValue.Should().BeNull();
        }

        [Fact]
        public static void LoadMethodWhenGetValueThrowsException()
        {
            var exception = new ArgumentNullException();

            var mockSecret1 = MockSecret.Get("foo", "abc");
            var mockSecret2 = MockSecret.Get("bar", exception);

            var caughtExceptions = new List<Exception>();

            void OnSecretException(SecretExceptionContext context)
            {
                caughtExceptions.Add(context.Exception);
            }

            var source = new SecretsConfigurationSource
            {
                Secrets = { mockSecret1.Object, mockSecret2.Object },
                ReloadMilliseconds = Timeout.Infinite,
                OnSecretException = OnSecretException
            };

            using var provider = new SecretsConfigurationProvider(source);

            provider.Load();

            provider.TryGet("foo", out var fooValue).Should().BeTrue();
            fooValue.Should().Be("abc");

            provider.TryGet("bar", out var barValue).Should().BeTrue();
            barValue.Should().BeNull();

            caughtExceptions.Should().HaveCount(1);
            caughtExceptions[0].Should().BeSameAs(exception);
        }
    }
}
