using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact(DisplayName = "AddRockLibSecrets method 1 gets the new source's SecretsProvider from config if defined")]
        public void AddRockLibSecretsMethod1HappyPath1()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { ["RockLib.Secrets:Type"] = typeof(TestSecretsProvider).AssemblyQualifiedName });

            builder.AddRockLibSecrets();

            var source = builder.Sources[1].Should().BeOfType<SecretsConfigurationSource>().Subject;
            source.SecretsProvider.Should().BeOfType<TestSecretsProvider>();
        }

        [Fact(DisplayName = "AddRockLibSecrets method 1 does not set new source's SecretsProvider if config does not define a secrets provider")]
        public void AddRockLibSecretsMethod1HappyPath2()
        {
            var builder = new ConfigurationBuilder();

            builder.AddRockLibSecrets();

            var source = builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>().Subject;
            source.SecretsProvider.Should().BeNull();
        }

        [Fact(DisplayName = "AddRockLibSecrets method 1 throws if builder is null")]
        public void AddRockLibSecretsMethod1SadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.AddRockLibSecrets();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "AddRockLibSecrets method 1 gets the new source's SecretsProvider from the parameter if not null")]
        public void AddRockLibSecretsMethod2HappyPath1()
        {
            var secretsProvider = new Mock<ISecretsProvider>().Object;

            var builder = new ConfigurationBuilder();

            builder.AddRockLibSecrets(secretsProvider);

            var source = builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>().Subject;
            source.SecretsProvider.Should().BeSameAs(secretsProvider);
        }

        [Fact(DisplayName = "AddRockLibSecrets method 1 does not set new source's SecretsProvider if parameter is null")]
        public void AddRockLibSecretsMethod2HappyPath2()
        {
            var builder = new ConfigurationBuilder();

            builder.AddRockLibSecrets((ISecretsProvider)null);

            var source = builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>().Subject;
            source.SecretsProvider.Should().BeNull();
        }

        [Fact(DisplayName = "AddRockLibSecrets method 2 throws if builder is null")]
        public void AddRockLibSecretsMethod2SadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.AddRockLibSecrets((ISecretsProvider)null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "AddRockLibSecrets method 3 invokes the configureSource parameter with the new source")]
        public void AddRockLibSecretsMethod3HappyPath1()
        {
            var secretsProvider = new Mock<ISecretsProvider>().Object;

            Action<SecretsConfigurationSource> configureSource = source =>
            {
                source.SecretsProvider = secretsProvider;
            };

            var builder = new ConfigurationBuilder();

            builder.AddRockLibSecrets(configureSource);

            var source = builder.Sources[0].Should().BeOfType<SecretsConfigurationSource>().Subject;
            source.SecretsProvider.Should().BeSameAs(secretsProvider);
        }

        [Fact(DisplayName = "AddRockLibSecrets method 3 throws if builder is null")]
        public void AddRockLibSecretsMethod3SadPath()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.AddRockLibSecrets((Action<SecretsConfigurationSource>)null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "SetSecretsProvider method sets builder.Properties with 'RockLib.SecretsProvider' and the specified secrets provider")]
        public void SetSecretsProviderMethodHappyPath()
        {
            var secretsProvider = new Mock<ISecretsProvider>().Object;

            var builder = new ConfigurationBuilder();

            builder.SetSecretsProvider(secretsProvider);

            builder.Properties[ConfigurationBuilderExtensions.SecretsProviderKey].Should().BeSameAs(secretsProvider);
        }

        [Fact(DisplayName = "SetSecretsProvider method throws if builder is null")]
        public void SetSecretsProviderMethodSadPath1()
        {
            IConfigurationBuilder builder = null;
            var secretsProvider = new Mock<ISecretsProvider>().Object;

            Action act = () => builder.SetSecretsProvider(secretsProvider);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "SetSecretsProvider method throws if secretsProvider is null")]
        public void SetSecretsProviderMethodSadPath2()
        {
            var builder = new ConfigurationBuilder();

            Action act = () => builder.SetSecretsProvider(null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*secretsProvider*");
        }

        [Fact(DisplayName = "GetSecretsProvider method returns the secrets provider from builder.Properties[\"RockLib.SecretsProvider\"]")]
        public void GetSecretsProviderMethodHappyPath()
        {
            var builder = new ConfigurationBuilder();
            var expectedSecretsProvider = new Mock<ISecretsProvider>().Object;

            builder.Properties.Add(ConfigurationBuilderExtensions.SecretsProviderKey, expectedSecretsProvider);

            var secretsProvider = builder.GetSecretsProvider();

            secretsProvider.Should().BeSameAs(expectedSecretsProvider);
        }

        [Fact(DisplayName = "GetSecretsProvider method throws if builder is null")]
        public void GetSecretsProviderMethodSadPath1()
        {
            IConfigurationBuilder builder = null;

            Action act = () => builder.GetSecretsProvider();

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact(DisplayName = "GetSecretsProvider method throws if builder.Properties does not have a 'RockLib.SecretsProvider' item")]
        public void GetSecretsProviderMethodSadPath2()
        {
            var builder = new ConfigurationBuilder();

            Action act = () => builder.GetSecretsProvider();

            act.Should().ThrowExactly<KeyNotFoundException>();
        }

        class TestSecretsProvider : ISecretsProvider
        {
            public IReadOnlyList<ISecret> Secrets { get; } = new ISecret[0];
        }
    }
}
