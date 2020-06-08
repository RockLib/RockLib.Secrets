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
    public partial class AwsSecretTests
    {
        [Fact(DisplayName = "Constructor sets properties")]
        public void ConstructorHappyPath1()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("key", "awsSecretName", "awsSecretKey",  secretsManager);

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().Be("awsSecretKey");
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact(DisplayName = "Constructor does not require awsSecretKey parameter")]
        public void ConstructorHappyPath2()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("key", "awsSecretName", secretsManager: secretsManager);

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().BeNull();
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact(DisplayName = "Constructor sets SecretsManager to DefaultSecretsManager if not specified")]
        public void ConstructorHappyPath3()
        {
            var secret = new AwsSecret("key", "awsSecretName", "awsSecretKey");

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().Be("awsSecretKey");
            secret.SecretsManager.Should().BeSameAs(AwsSecret.DefaultSecretsManager);
        }

        [Fact(DisplayName = "Constructor throws if key is null")]
        public void ConstructorSadPath1()
        {
            Action act = () => new AwsSecret(null, "awsSecretName");

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*key*");
        }

        [Fact(DisplayName = "Constructor throws if awsSecretName is null")]
        public void ConstructorSadPath2()
        {
            Action act = () => new AwsSecret("key", null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*awsSecretName*");
        }

        [Fact(DisplayName = "DefaultSecretsManager property setter works as expected")]
        public void DefaultSecretsManagerPropertySetterHappyPath()
        {
            var current = AwsSecret.DefaultSecretsManager;
            try
            {
                var secretsManager = new Mock<IAmazonSecretsManager>().Object;

                AwsSecret.DefaultSecretsManager = secretsManager;

                AwsSecret.DefaultSecretsManager.Should().BeSameAs(secretsManager);
            }
            finally
            {
                AwsSecret.DefaultSecretsManager = current;
            }
        }

        [Fact(DisplayName = "DefaultSecretsManager property setter throws if value is null")]
        public void DefaultSecretsManagerPropertySetterSadPath()
        {
            Action act = () => AwsSecret.DefaultSecretsManager = null;

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*value*");
        }

        [Fact(DisplayName = "DefaultSecretsManager property has a default value of type AmazonSecretsManagerClient")]
        public void DefaultSecretsManagerPropertyDefaultValue()
        {
            var current = AwsSecret.DefaultSecretsManager;

            try
            {
                typeof(AwsSecret).Unlock()._defaultSecretsManager = null;

                AwsSecret.DefaultSecretsManager.Should().BeOfType<AmazonSecretsManagerClient>();
            }
            finally
            {
                AwsSecret.DefaultSecretsManager = current;
            }
        }

        [Fact(DisplayName = "GetValue method returns the value of the secret key when supplied")]
        public void GetValueMethodHappyPath1()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "{'myAwsSecretKey':'secretValue'}"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myKey", "myAwsSecretName", "myAwsSecretKey", mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be("secretValue");

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "myAwsSecretName"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact(DisplayName = "GetValue method returns the entire SecretString if AwsSecretKey is not supplied")]
        public void GetValueMethodHappyPath2()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "mySecretString"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myKey", "myAwsSecretName", secretsManager: mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be("mySecretString");

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "myAwsSecretName"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact(DisplayName = "GetValue method returns the base64 encoded SecretBinary if SecretString is null")]
        public void GetValueMethodHappyPath3()
        {
            var buffer = Encoding.UTF8.GetBytes("Hello, world!");

            var response = new GetSecretValueResponse
            {
                SecretBinary = new MemoryStream(buffer)
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myKey", "myAwsSecretName", secretsManager: mockSecretsManager.Object);

            var value = secret.GetValue();

            value.Should().Be(Convert.ToBase64String(buffer));

            mockSecretsManager.Verify(m => m.GetSecretValueAsync(
                It.Is<GetSecretValueRequest>(r => r.SecretId == "myAwsSecretName"), It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact(DisplayName = "GetValue method throws if SecretsManager returns null response")]
        public void GetValueMethodSadPath1()
        {
            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetSecretValueResponse)null);

            var secret = new AwsSecret("myKey", "myAwsSecretName", "myAwsSecretKey", mockSecretsManager.Object);

            Action act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response was null.*");
        }

        [Fact(DisplayName = "GetValue method throws if SecretString does not contain the AwsSecretKey")]
        public void GetValueMethodSadPath2()
        {
            var response = new GetSecretValueResponse
            {
                SecretString = "{}"
            };

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myKey", "myAwsSecretName", "myAwsSecretKey", mockSecretsManager.Object);

            Action act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response did not contain item with the name 'myAwsSecretKey'.*");
        }

        [Fact(DisplayName = "GetValue method throws if SecretString and SecretBinary are both null")]
        public void GetValueMethodSadPath3()
        {
            var response = new GetSecretValueResponse();

            var mockSecretsManager = new Mock<IAmazonSecretsManager>();
            mockSecretsManager.Setup(m => m.GetSecretValueAsync(It.IsAny<GetSecretValueRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var secret = new AwsSecret("myKey", "myAwsSecretName", "myAwsSecretKey", mockSecretsManager.Object);

            Action act = () => secret.GetValue();

            act.Should().ThrowExactly<KeyNotFoundException>().WithMessage("*Response did not contain a value for SecretString or SecretBinary.*");
        }
    }
}
