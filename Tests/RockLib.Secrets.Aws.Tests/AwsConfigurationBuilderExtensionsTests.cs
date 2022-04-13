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
        [Fact(DisplayName = "SetAmazonSecretsManager extension method sets AwsSecret.DefaultSecretsManager")]
        public static void SetAmazonSecretsManagerMethodHappyPath()
        {
            var current = AwsSecret.DefaultSecretsManager;

            try
            {
                var amazonSecretsManager = new Mock<IAmazonSecretsManager>().Object;

                IConfigurationBuilder builder = new ConfigurationBuilder();

                var returnBuilder = builder.SetAmazonSecretsManager(amazonSecretsManager);

                returnBuilder.Should().BeSameAs(builder);
                AwsSecret.DefaultSecretsManager.Should().BeSameAs(amazonSecretsManager);
            }
            finally
            {
                AwsSecret.DefaultSecretsManager = current;
            }
        }

        [Fact(DisplayName = "SetAmazonSecretsManager extension method throws if secretsManager is null")]
        public static void SetAmazonSecretsManagerMethodSadPath()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            Action act = () => builder.SetAmazonSecretsManager(null!);

            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*secretsManager*");
        }
    }
}
