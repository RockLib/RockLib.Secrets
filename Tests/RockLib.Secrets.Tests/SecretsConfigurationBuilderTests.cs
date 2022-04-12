using FluentAssertions;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public static class SecretsConfigurationBuilderTests
    {
        [Fact]
        public static void Create()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            builder.Source.Should().BeSameAs(source);
        }

        [Fact]
        public static void ConstructorWithNullSource()
        {
            var act = () => new SecretsConfigurationBuilder(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public static void AddSecretMethod()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            var secret = MockSecret.Get("foo", "bar").Object;

            builder.AddSecret(secret);

            source.Secrets.Should().ContainSingle()
                .Which.Should().BeSameAs(secret);
        }

        [Fact]
        public static void AddSecretMethodWithNullSecret()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            var act = () => builder.AddSecret(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*secret*");
        }
    }
}
