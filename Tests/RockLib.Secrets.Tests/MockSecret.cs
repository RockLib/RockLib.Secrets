using Moq;
using System;

namespace RockLib.Secrets.Tests
{
    internal static class MockSecret
    {
        /// <summary>
        /// Gets a mock secret that returns the specified value.
        /// </summary>
        public static Mock<ISecret> Get(string key, string value)
        {
            var mockSecret = new Mock<ISecret>();
            mockSecret.Setup(m => m.ConfigurationKey).Returns(key);
            mockSecret.Setup(m => m.GetValue()).Returns(value);
            return mockSecret;
        }

        /// <summary>
        /// Gets a mock secret that throws the specified exception.
        /// </summary>
        public static Mock<ISecret> Get(string key, Exception exception)
        {
            var mockSecret = new Mock<ISecret>();
            mockSecret.Setup(m => m.ConfigurationKey).Returns(key);
            mockSecret.Setup(m => m.GetValue()).Throws(exception);
            return mockSecret;
        }
    }
}
