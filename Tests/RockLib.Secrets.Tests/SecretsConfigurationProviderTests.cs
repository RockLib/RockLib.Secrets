using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class SecretsConfigurationProviderTests
    {
        [Fact(DisplayName = "Constructor sets properties")]
        public void ConstructorHappyPath()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key2", "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Source.Should().BeSameAs(source);

            provider.Secrets.Should().NotBeSameAs(source.Secrets);
            provider.Secrets.Should().BeEquivalentTo(source.Secrets);
        }

        [Fact(DisplayName = "Constructor throws when source parameter is null")]
        public void ConstructorSadPath1()
        {
            Action act = () => new SecretsConfigurationProvider(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.Secrets contains no items")]
        public void ConstructorSadPath2()
        {
            var source = new SecretsConfigurationSource();

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource must contain at least one secret.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.Secrets contains null items")]
        public void ConstructorSadPath3()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            ISecret secret2 = null;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any null secrets.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.Secrets contains items with null Key")]
        public void ConstructorSadPath4()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get(null, "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any secrets with a null Key.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider.Secrets contains items with duplicate keys")]
        public void ConstructorSadPath5()
        {
            var secret1 = MockSecret.Get("key1", "value1").Object;
            var secret2 = MockSecret.Get("key1", "value2").Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 }
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("The SecretsConfigurationSource cannot contain any secrets with duplicate Keys.*source*");
        }

        [Fact(DisplayName = "Load method adds each secret's key and value to the provider's Data")]
        public void LoadMethodHappyPath()
        {
            var secret1 = MockSecret.Get("foo", "abc").Object;
            var secret2 = MockSecret.Get("bar", (string)null).Object;

            var source = new SecretsConfigurationSource
            {
                Secrets = { secret1, secret2 },
                ReloadMilliseconds = Timeout.Infinite
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Load();

            provider.TryGet("foo", out var fooValue).Should().BeTrue();
            fooValue.Should().Be("abc");

            provider.TryGet("bar", out var barValue).Should().BeTrue();
            barValue.Should().BeNull();
        }

        [Fact(DisplayName = "Load method invokes exception handler when ISecret.GetValue method throws")]
        public void LoadMethodSadPath()
        {
            var exception = new Exception();

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

            var provider = new SecretsConfigurationProvider(source);

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
