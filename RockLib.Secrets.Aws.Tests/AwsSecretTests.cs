using Amazon.SecretsManager;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using RockLib.Dynamic;
using System;
using Xunit;

namespace RockLib.Secrets.Aws.Tests
{
    public partial class AwsSecretTests
    {
        [Fact(DisplayName = null)]
        public void Constructor1HappyPath1()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("key", "awsSecretName", secretsManager);

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().BeNull();
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact(DisplayName = null)]
        public void Constructor1HappyPath2()
        {
            var secret = new AwsSecret("key", "awsSecretName");

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().BeNull();
            secret.SecretsManager.Should().BeSameAs(AwsSecret.DefaultSecretsManager);
        }

        [Fact(DisplayName = null)]
        public void Constructor1SadPath1()
        {
            Action act = () => new AwsSecret(null, "awsSecretName");

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*key*");
        }

        [Fact(DisplayName = null)]
        public void Constructor1SadPath2()
        {
            Action act = () => new AwsSecret("key", null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*awsSecretName*");
        }

        [Fact(DisplayName = null)]
        public void Constructor2HappyPath1()
        {
            var secretsManager = new Mock<IAmazonSecretsManager>().Object;

            var secret = new AwsSecret("key", "awsSecretName", "awsSecretKey",  secretsManager);

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().Be("awsSecretKey");
            secret.SecretsManager.Should().BeSameAs(secretsManager);
        }

        [Fact(DisplayName = null)]
        public void Constructor2HappyPath2()
        {
            var secret = new AwsSecret("key", "awsSecretName", "awsSecretKey");

            secret.Key.Should().Be("key");
            secret.AwsSecretName.Should().Be("awsSecretName");
            secret.AwsSecretKey.Should().Be("awsSecretKey");
            secret.SecretsManager.Should().BeSameAs(AwsSecret.DefaultSecretsManager);
        }

        [Fact(DisplayName = null)]
        public void Constructor2SadPath1()
        {
            Action act = () => new AwsSecret(null, "awsSecretName");

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*key*");
        }

        [Fact(DisplayName = null)]
        public void Constructor2SadPath2()
        {
            Action act = () => new AwsSecret("key", null);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*awsSecretName*");
        }

        [Fact(DisplayName = null)]
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

        [Fact(DisplayName = null)]
        public void DefaultSecretsManagerPropertySetterSadPath()
        {
            Action act = () => AwsSecret.DefaultSecretsManager = null;

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*value*");
        }

        [Fact(DisplayName = null)]
        public void DefaultSecretsManagerPropertyDefaultValue()
        {
            // use uma
            var current = AwsSecret.DefaultSecretsManager;

            try
            {
                typeof(AwsSecret).Unlock()._defaultSecretsManager = null;

                // TODO: Is there any way of knowing that it's an instance initialized with the default constructor?
                AwsSecret.DefaultSecretsManager.Should().BeOfType<AmazonSecretsManagerClient>();
            }
            finally
            {
                AwsSecret.DefaultSecretsManager = current;
            }
        }

        [Fact(DisplayName = null)]
        public void GetValueMethodHappyPath1()
        {
            
        }

        [Fact(DisplayName = null)]
        public void GetValueMethodHappyPath2()
        {

        }

        [Fact(DisplayName = null)]
        public void GetValueMethodHappyPath3()
        {

        }

        [Fact(DisplayName = null)]
        public void GetValueMethodSadPath1()
        {

        }

        [Fact(DisplayName = null)]
        public void GetValueMethodSadPath2()
        {

        }

        [Fact(DisplayName = null)]
        public void GetValueMethodSadPath3()
        {

        }
    }
}
