using FluentAssertions;
using Moq;
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
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[0]);

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            var provider = new SecretsConfigurationProvider(source);

            provider.Source.Should().BeSameAs(source);
        }

        [Fact(DisplayName = "Constructor throws when source parameter is null")]
        public void ConstructorSadPath1()
        {
            Action act = () => new SecretsConfigurationProvider(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider is null")]
        public void ConstructorSadPath2()
        {
            var source = new SecretsConfigurationSource
            {
                SecretsProvider = null
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("SecretsProvider cannot be null.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider.Secrets is null")]
        public void ConstructorSadPath3()
        {
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns((IReadOnlyList<ISecret>)null);

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("SecretsProvider.Secrets cannot be null.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider.Secrets contains null items")]
        public void ConstructorSadPath4()
        {
            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[] { null });

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("SecretsProvider.Secrets cannot contain any null items.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider.Secrets contains items with null Key")]
        public void ConstructorSadPath5()
        {
            var mockSecret = new Mock<ISecret>();
            mockSecret.Setup(m => m.Key).Returns((string)null);

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[] { mockSecret.Object });

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("SecretsProvider.Secrets cannot contain any items with a null Key.*source*");
        }

        [Fact(DisplayName = "Constructor throws when source.SecretsProvider.Secrets contains items with duplicate keys")]
        public void ConstructorSadPath6()
        {
            var mockSecret1 = new Mock<ISecret>();
            mockSecret1.Setup(m => m.Key).Returns("foo");

            var mockSecret2 = new Mock<ISecret>();
            mockSecret2.Setup(m => m.Key).Returns("foo");

            var mockSecretsProvider = new Mock<ISecretsProvider>();
            mockSecretsProvider.Setup(m => m.Secrets).Returns(new ISecret[] { mockSecret1.Object, mockSecret2.Object });

            var source = new SecretsConfigurationSource
            {
                SecretsProvider = mockSecretsProvider.Object
            };

            Action act = () => new SecretsConfigurationProvider(source);

            act.Should().ThrowExactly<ArgumentException>().WithMessage("SecretsProvider.Secrets cannot contain any items with duplicate Keys.*source*");
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
