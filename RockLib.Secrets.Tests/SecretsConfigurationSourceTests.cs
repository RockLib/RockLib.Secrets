using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
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
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[0]);

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            var builder = new ConfigurationBuilder();

            var provider = source.Build(builder);

            provider.Should().BeOfType<SecretsConfigurationProvider>()
                .Which.Source.Should().BeSameAs(source);
        }

        [Fact(DisplayName = "Build method sets SecretsProvider property from builder when ServiceProvider is null")]
        public void BuildMethodHappyPath2()
        {
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[0]);

            var source = new SecretsConfigurationSource();

            var builder = new ConfigurationBuilder();
            builder.SetSecretsProvider(mockSecretsProvider.Object);

            var provider = source.Build(builder);

            var secretsConfigurationProvider = provider.Should().BeOfType<SecretsConfigurationProvider>().Subject;
            secretsConfigurationProvider.Source.Should().BeSameAs(source);
            secretsConfigurationProvider.Source.SecretsProvider.Should().BeSameAs(mockSecretsProvider.Object);
        }

        [Fact(DisplayName = "Build method sets OnSecretException property from builder when OnSecretException is null")]
        public void BuildMethodHappyPath3()
        {
            Action<SecretExceptionContext> onSecretException = context => { };

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[0]);

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            var builder = new ConfigurationBuilder();
            builder.SetSecretExceptionHandler(onSecretException);

            var provider = source.Build(builder);

            var secretsConfigurationProvider = provider.Should().BeOfType<SecretsConfigurationProvider>().Subject;
            secretsConfigurationProvider.Source.Should().BeSameAs(source);
            secretsConfigurationProvider.Source.OnSecretException.Should().BeSameAs(onSecretException);
        }

        [Fact(DisplayName = "Build method throws when no SecretsProvider is provided")]
        public void BuildMethodSadPath()
        {
            var source = new SecretsConfigurationSource();
            var builder = new ConfigurationBuilder();

            Action act = () => source.Build(builder);

            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("No secrets provider was provided.");
        }
    }
}
