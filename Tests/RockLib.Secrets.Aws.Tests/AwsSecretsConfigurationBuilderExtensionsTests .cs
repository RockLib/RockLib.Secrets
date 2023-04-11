using Amazon.SecretsManager;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace RockLib.Secrets.Aws.Tests
{
    public static partial class AwsSecretTests
    {
        [Fact]
        public static void AddAwsSecret()
        {
            ISecret secret = null!;
            var builderMock = new Mock<ISecretsConfigurationBuilder>();
            builderMock
                .Setup(bm => bm.AddSecret(It.IsAny<ISecret>()))
                .Callback<ISecret>(s => secret = s);

            var configurationKey = "configurationKey";
            var secretId = "secretId";
            var secretKey = "secretKey";
            var awsSecretsManager = new Mock<IAmazonSecretsManager>().Object;

            builderMock.Object.AddAwsSecret(configurationKey, secretId, secretKey, awsSecretsManager);

            builderMock.Verify();

            var awsSecret = secret.Should().BeOfType<AwsSecret>().Subject;
            awsSecret.ConfigurationKey.Should().Be(configurationKey);
            awsSecret.SecretId.Should().Be(secretId);
            awsSecret.SecretKey.Should().Be(secretKey);
            awsSecret.SecretsManager.Should().BeSameAs(awsSecretsManager);
        }

        [Fact]
        public static void AddAwsSecretWithNullBuilder()
        {
            var configurationKey = "configurationKey";
            var secretId = "secretId";
            var secretKey = "secretKey";
            var awsSecretsManager = new Mock<IAmazonSecretsManager>().Object;

            ISecretsConfigurationBuilder builder = null!;

            Action act = () => builder.AddAwsSecret(configurationKey, secretId, secretKey, awsSecretsManager);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }

        [Fact]
        public static void AddAwsSecretsWithRockLibConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json")
                .AddRockLibSecrets();
            var buildAction = () => configurationBuilder.Build();
            buildAction.Should().NotThrow();
        }
    }
}
