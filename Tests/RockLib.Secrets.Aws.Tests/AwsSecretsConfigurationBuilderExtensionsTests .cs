using Amazon.SecretsManager;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace RockLib.Secrets.Aws.Tests
{
    public static partial class AwsSecretTests
    {
        [Fact(DisplayName = "AddAwsSecret calls AddSecret with AwsSecret with all passed in values")]
        public static void AddAwsSecretHappyPath()
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

            builderMock.Verify(bm => bm.AddSecret(It.IsAny<ISecret>()), Times.Once);

            var awsSecret = secret.Should().BeOfType<AwsSecret>().Subject;
            awsSecret.ConfigurationKey.Should().Be(configurationKey);
            awsSecret.SecretId.Should().Be(secretId);
            awsSecret.SecretKey.Should().Be(secretKey);
            awsSecret.SecretsManager.Should().BeSameAs(awsSecretsManager);
        }

        [Fact(DisplayName = "AddAwsSecret throws is builder is null")]
        public static void AddAwsSecretSadPath()
        {
            var configurationKey = "configurationKey";
            var secretId = "secretId";
            var secretKey = "secretKey";
            var awsSecretsManager = new Mock<IAmazonSecretsManager>().Object;

            ISecretsConfigurationBuilder builder = null!;

            Action act = () => builder.AddAwsSecret(configurationKey, secretId, secretKey, awsSecretsManager);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }
    }
}
