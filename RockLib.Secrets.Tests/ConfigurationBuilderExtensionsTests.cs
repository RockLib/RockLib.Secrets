using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact(DisplayName = "AddRockLibSecrets method 1 adds a secrets source and returns a secrets builder")]
        public void AddRockLibSecretsMethod1HappyPath()
        {
            var builder = new ConfigurationBuilder();

            builder.Sources.Should().BeEmpty();

            var secretsBuilder = builder.AddRockLibSecrets();

            builder.Sources.Should().ContainSingle();
            builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>();

            secretsBuilder.Should().BeOfType<SecretsConfigurationBuilder>()
                .Which.Source.Should().BeSameAs(builder.Sources[0]);
        }

        [Fact(DisplayName = "AddRockLibSecrets method 1 throws if builder is null")]
        public void AddRockLibSecretsMethod1SadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.AddRockLibSecrets();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "AddRockLibSecrets method 2 adds a secrets source, configures it, and returns a secrets builder")]
        public void AddRockLibSecretsMethod2HappyPath()
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

        [Fact(DisplayName = "AddRockLibSecrets method 2 throws if builder is null")]
        public void AddRockLibSecretsMethod2SadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.AddRockLibSecrets(source => { });

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "SetSecretExceptionHandler method sets builder.Properties with 'RockLib.SecretsProvider' and the specified secrets provider")]
        public void SetSecretExceptionHandlerMethodHappyPath()
        {
            Action<SecretExceptionContext> onSecretException = context => { };

            var builder = new ConfigurationBuilder();

            builder.SetSecretExceptionHandler(onSecretException);

            builder.Properties[ConfigurationBuilderExtensions.SecretExceptionHandlerKey].Should().BeSameAs(onSecretException);
        }

        [Fact(DisplayName = "SetSecretExceptionHandler method throws if builder is null")]
        public void SetSecretExceptionHandlerMethodSadPath1()
        {
            IConfigurationBuilder builder = null;
            Action<SecretExceptionContext> onSecretException = context => { };

            Action act = () => builder.SetSecretExceptionHandler(onSecretException);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "SetSecretExceptionHandler method throws if secretsProvider is null")]
        public void SetSecretExceptionHandlerMethodSadPath2()
        {
            var builder = new ConfigurationBuilder();

            Action act = () => builder.SetSecretExceptionHandler(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*onSecretException*");
        }

        [Fact(DisplayName = "GetSecretExceptionHandler method returns the secrets provider from builder.Properties[\"RockLib.SecretsProvider\"]")]
        public void GetSecretExceptionHandlerMethodHappyPath1()
        {
            var builder = new ConfigurationBuilder();
            Action<SecretExceptionContext> onSecretException = context => { };

            builder.Properties.Add(ConfigurationBuilderExtensions.SecretExceptionHandlerKey, onSecretException);

            var secretsProvider = builder.GetSecretExceptionHandler();

            secretsProvider.Should().BeSameAs(onSecretException);
        }

        [Fact(DisplayName = "GetSecretExceptionHandler method returns null if builder.Properties does not have a 'RockLib.SecretExceptionHandler' item")]
        public void GetSecretExceptionHandlerMethodHappyPath2()
        {
            var builder = new ConfigurationBuilder();

            var onSecretException = builder.GetSecretExceptionHandler();

            onSecretException.Should().BeNull();
        }

        [Fact(DisplayName = "GetSecretExceptionHandler method throws if builder is null")]
        public void GetSecretExceptionHandlerMethodSadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.GetSecretExceptionHandler();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }
    }
}
