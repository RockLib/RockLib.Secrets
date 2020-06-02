using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class SecretsConfigurationSourceTests
    {
        [Fact(DisplayName = "Build method returns SecretsConfigurationProvider")]
        public void BuildMethodHappyPath1()
        {
            var source = new SecretsConfigurationSource
            {
                SecretsProvider = new Mock<ISecretsProvider>().Object
            };

            var builder = new ConfigurationBuilder();

            var provider = source.Build(builder);

            provider.Should().BeOfType<SecretsConfigurationProvider>()
                .Which.Source.Should().BeSameAs(source);
        }

        [Fact(DisplayName = "Build method sets SecretsProvider property from builder when ServiceProvider is null")]
        public void BuildMethodHappyPath2()
        {
            var secretsProvider = new Mock<ISecretsProvider>().Object;

            var source = new SecretsConfigurationSource();

            var builder = new ConfigurationBuilder();
            builder.SetSecretsProvider(secretsProvider);

            var provider = source.Build(builder);

            var secretsConfigurationProvider = provider.Should().BeOfType<SecretsConfigurationProvider>().Subject;
            secretsConfigurationProvider.Source.Should().BeSameAs(source);
            secretsConfigurationProvider.Source.SecretsProvider.Should().BeSameAs(secretsProvider);
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
