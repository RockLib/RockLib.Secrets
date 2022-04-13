using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using FluentAssertions;
using Moq;
using RockLib.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Secrets.Aws.Tests
{
    public static partial class AwsSecretTests
    {
        [Fact]
        public static void Create()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("configurationKey", "secretId", "secretKey",  secretsManager);

            secret.ConfigurationKey.Should().Be("configurationKey");
            secret.SecretId.Should().Be("secretId");
            secret.SecretKey.Should().Be("secretKey");
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact]
        public static void CreateWithNoSecretKey()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("configurationKey", "secretId", null, secretsManager);

            secret.ConfigurationKey.Should().Be("configurationKey");
            secret.SecretId.Should().Be("secretId");
            secret.SecretKey.Should().BeNull();
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact]
        public static void CreateWithNullConfigurationKey()
        {
            var act = () => new AwsSecret(null!, "secretId", null, Mock.Of<IAmazonSecretsManager>());

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*configurationKey*");
        }

        [Fact]
        public static void CreateWithNullSecretId()
        {
            var act = () => new AwsSecret("configurationKey", null!, null, Mock.Of<IAmazonSecretsManager>());

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*secretId*");
        }

        [Fact]
        public static void GetValue()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "{'myAwsSecretKey':'secretValue'}"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", "myAwsSecretKey", mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be("secretValue");

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "mySecretId"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public static void GetValueWhenSecretKeyIsNotSupplied()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "mySecretString"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", null, mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be("mySecretString");

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "mySecretId"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public static void GetValueWhenSecretStringIsNull()
        {
            var buffer = Encoding.UTF8.GetBytes("Hello, world!");

            var response = new GetSecretValueResponse
            {
                SecretBinary = new MemoryStream(buffer)
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", null, mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be(Convert.ToBase64String(buffer));

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "mySecretId"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public static void GetValueWithNullResponseFromSecretsManager()
        {
            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetSecretValueResponse)null!);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", "myAwsSecretKey", mockSecretsManager.Object);

            var act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response was null.*");
        }

        [Fact]
        public static void GetValueWhenSecretStringDoesNotContainSecretKey()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "{}"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", "myAwsSecretKey", mockSecretsManager.Object);

            var act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response did not contain item with the name 'myAwsSecretKey'.*");
        }

        [Fact]
        public static void GetValueWhenSecretStringandSecretBinaryAreNull()
        {
            var response = new GetSecretValueResponse();

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myConfigurationKey", "mySecretId", "myAwsSecretKey", mockSecretsManager.Object);

            var act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response did not contain a value for SecretString or SecretBinary.*");
        }
    }
}
