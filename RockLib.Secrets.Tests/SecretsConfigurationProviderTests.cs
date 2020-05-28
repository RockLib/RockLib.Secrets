using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class SecretsConfigurationProviderTests
    {
        [Fact(DisplayName = "Constructor sets properties")]
        public void ConstructorHappyPath()
        {
            var source = new SecretsConfigurationSource
            {
                SecretsProvider = new Mock<ISecretsProvider>().Object
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Source.Should().BeSameAs(source);
        }

        [Fact(DisplayName = "Constructor throws when 'source' parameter is null")]
        public void ConstructorSadPath()
        {
            Action act = () => new SecretsConfigurationProvider(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact(DisplayName = "Load method adds each secret's key and value to the provider's Data")]
        public void LoadMethodHappyPath()
        {
            var mockSecret1 = new Mock<ISecret>();
            mockSecret1.Setup(m => m.Key).Returns("foo");
            mockSecret1.Setup(m => m.GetValue()).Returns("abc");

            var mockSecret2 = new Mock<ISecret>();
            mockSecret2.Setup(m => m.Key).Returns("bar");
            mockSecret2.Setup(m => m.GetValue()).Returns("123");

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new[] { mockSecret1.Object, mockSecret2.Object });

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Load();

            provider.TryGet("foo", out var fooValue).Should().BeTrue();
            fooValue.Should().Be("abc");

            provider.TryGet("bar", out var barValue).Should().BeTrue();
            barValue.Should().Be("123");
        }

        [Fact(DisplayName = "Load method ignores secrets that throw from their GetValue() method")]
        public void LoadMethodSadPath()
        {
            var mockSecret1 = new Mock<ISecret>();
            mockSecret1.Setup(m => m.Key).Returns("foo");
            mockSecret1.Setup(m => m.GetValue()).Returns("abc");

            var mockSecret2 = new Mock<ISecret>();
            mockSecret2.Setup(m => m.Key).Returns("bar");
            mockSecret2.Setup(m => m.GetValue()).Throws<Exception>();

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new[] { mockSecret1.Object, mockSecret2.Object });

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Load();

            provider.TryGet("foo", out var fooValue).Should().BeTrue();
            fooValue.Should().Be("abc");

            provider.TryGet("bar", out _).Should().BeFalse();
        }
    }
}
