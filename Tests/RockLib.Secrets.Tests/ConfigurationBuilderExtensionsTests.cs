using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public static class ConfigurationBuilderExtensionsTests
    {
        [Fact]
        public static void AddRockLibSecrets()
        {
            var builder = new ConfigurationBuilder();

            builder.Sources.Should().BeEmpty();

            var secretsBuilder = builder.AddRockLibSecrets();

            builder.Sources.Should().ContainSingle();
            builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>();

            secretsBuilder.Should().BeOfType<SecretsConfigurationBuilder>()
                .Which.Source.Should().BeSameAs(builder.Sources[0]);
        }

        [Fact]
        public static void AddRockLibSecretsWithNullBuilder()
        {
            IConfigurationBuilder builder = null!;

            var act = () => builder.AddRockLibSecrets();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact]
        public static void AddRockLibSecretsWithConfiguration()
        {
            var builder = new ConfigurationBuilder();

            builder.Sources.Should().BeEmpty();

            var secret = MockSecret.Get("foo", "bar").Object;

            Action<SecretsConfigurationSource> configureSource = source => source.Secrets.Add(secret);

            var secretsBuilder = builder.AddRockLibSecrets(configureSource);

            builder.Sources.Should().ContainSingle();
            builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>()
                .Which.Secrets.Should().ContainSingle(s => ReferenceEquals(s, secret));

            secretsBuilder.Should().BeOfType<SecretsConfigurationBuilder>()
                .Which.Source.Should().BeSameAs(builder.Sources[0]);
        }

        [Fact]
        public static void AddRockLibSecretsWithSourceAndNullBuilder()
        {
            IConfigurationBuilder builder = null!;

            Action act = () => builder.AddRockLibSecrets(source => { });

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact]
        public static void SetSecretExceptionHandlerMethod()
        {
            Action<SecretExceptionContext> onSecretException = context => { };

            var builder = new ConfigurationBuilder();

            builder.SetSecretExceptionHandler(onSecretException);

            builder.Properties[ConfigurationBuilderExtensions.SecretExceptionHandlerKey].Should().BeSameAs(onSecretException);
        }

        [Fact]
        public static void SetSecretExceptionHandlerMethodWithNullBuilder()
        {
            IConfigurationBuilder builder = null!;
            Action<SecretExceptionContext> onSecretException = context => { };

            Action act = () => builder.SetSecretExceptionHandler(onSecretException);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact]
        public static void SetSecretExceptionHandlerMethodWithNullHandler()
        {
            var builder = new ConfigurationBuilder();

            Action act = () => builder.SetSecretExceptionHandler(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*onSecretException*");
        }

        [Fact]
        public static void GetSecretExceptionHandlerMethod()
        {
            var builder = new ConfigurationBuilder();
            Action<SecretExceptionContext> onSecretException = context => { };

            builder.Properties.Add(ConfigurationBuilderExtensions.SecretExceptionHandlerKey, onSecretException);

            var secretsProvider = builder.GetSecretExceptionHandler();

            secretsProvider.Should().BeSameAs(onSecretException);
        }

        [Fact]
        public static void GetSecretExceptionHandlerMethodWithNoHandler()
        {
            var builder = new ConfigurationBuilder();

            var onSecretException = builder.GetSecretExceptionHandler();

            onSecretException.Should().BeNull();
        }

        [Fact]
        public static void GetSecretExceptionHandlerMethodWithNullBuilder()
        {
            IConfigurationBuilder builder = null!;

            Action act = () => builder.GetSecretExceptionHandler();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }
    }
}
