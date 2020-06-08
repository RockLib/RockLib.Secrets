using Amazon.SecretsManager;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace RockLib.Secrets.Aws.Tests
{
    partial class AwsSecretTests
    {
        [Fact(DisplayName = "AddAwsSecret calls AddSecret with AwsSecret with all passed in values")]
        public void AddAwsSecretHappyPath()
        {
            ISecret secret = null;
            var builderMock = new Mock<ISecretsConfigurationBuilder>();
            builderMock
                .Setup(bm => bm.AddSecret(It.IsAny<ISecret>()))
                .Callback<ISecret>(s => secret = s);

            var key = "key";
            var awsSecretName = "awsSecretName";
            var awsSecretKey = "aswSecretKey";
            var awsSecretsManager = new Mock<IAmazonSecretsManager>().Object;

            builderMock.Object.AddAwsSecret(key, awsSecretName, awsSecretKey, awsSecretsManager);

            builderMock.Verify(bm => bm.AddSecret(It.IsAny<ISecret>()), Times.Once);

            var awsSecret = secret.Should().BeOfType<AwsSecret>().Subject;
            awsSecret.Key.Should().Be(key);
            awsSecret.AwsSecretName.Should().Be(awsSecretName);
            awsSecret.AwsSecretKey.Should().Be(awsSecretKey);
            awsSecret.SecretsManager.Should().BeSameAs(awsSecretsManager);
        }

        [Fact(DisplayName = "AddAwsSecret throws is builder is null")]
        public void AddAwsSecretSadPath()
        {
            var key = "key";
            var awsSecretName = "awsSecretName";
            var awsSecretKey = "aswSecretKey";
            var awsSecretsManager = new Mock<IAmazonSecretsManager>().Object;

            ISecretsConfigurationBuilder builder = null;

            Action act = () => builder.AddAwsSecret(key, awsSecretName, awsSecretKey, awsSecretsManager);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*builder*");
        }
    }
}
