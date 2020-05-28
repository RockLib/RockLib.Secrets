using FluentAssertions;
using Moq;
using Xunit;

namespace RockLib.Secrets.Tests
{
    public class CompositeSecretsProviderTests
    {
        [Fact(DisplayName = "Constructor sets properties")]
        public void ConstructorHappyPath()
        {
            var secret1 = new Mock<ISecret>().Object;
            var secret2 = new Mock<ISecret>().Object;
            var secret3 = new Mock<ISecret>().Object;
            var secret4 = new Mock<ISecret>().Object;

            var mockSecretProvider1 = new Mock<ISecretsProvider>();
            mockSecretProvider1.Setup(m => m.Secrets).Returns(new[] { secret1, secret2 });

            var mockSecretProvider2 = new Mock<ISecretsProvider>();
            mockSecretProvider2.Setup(m => m.Secrets).Returns(new[] { secret3, secret4 });

            var compositeSecretsProvider = new CompositeSecretsProvider(new[] { mockSecretProvider1.Object, mockSecretProvider2.Object });

            compositeSecretsProvider.Providers.Should().BeEquivalentTo(new[] { mockSecretProvider1.Object, mockSecretProvider2.Object });
            compositeSecretsProvider.Secrets.Should().BeEquivalentTo(new[] { secret1, secret2, secret3, secret4 });
        }
    }
}
