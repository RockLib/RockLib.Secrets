using FluentAssertions;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public static class SecretsConfigurationBuilderTests
    {
        [Fact(DisplayName = "Constructor sets Source property")]
        public static void ConstructorHappyPath()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            builder.Source.Should().BeSameAs(source);
        }

        [Fact(DisplayName = "Constructor throws if source is null")]
        public static void ConstructorSadPath()
        {
            var act = () => new SecretsConfigurationBuilder(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact(DisplayName = "AddSecret method adds secret to source")]
        public static void AddSecretMethodHappyPath()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            var secret = MockSecret.Get("foo", "bar").Object;

            builder.AddSecret(secret);

            source.Secrets.Should().ContainSingle()
                .Which.Should().BeSameAs(secret);
        }

        [Fact(DisplayName = "AddSecret method throws if secret is null")]
        public static void AddSecretMethodSadPath()
        {
            var source = new SecretsConfigurationSource();

            var builder = new SecretsConfigurationBuilder(source);

            var act = () => builder.AddSecret(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*secret*");
        }
    }
}
