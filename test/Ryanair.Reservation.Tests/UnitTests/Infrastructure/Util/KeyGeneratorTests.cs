using System.Text.RegularExpressions;
using Ryanair.Reservation.Infrastructure.Business.Util;
using Xunit;

namespace Ryanair.Reservation.Tests.UnitTests.InfrastructureBusiness.Util
{
    public class KeyGeneratorTests
    {
        [Fact]
        public void ShouldGenerateValidKey()
        {
            // Arrange
            const string PATTERN_CHECK = "^[a-zA-Z]{3}[0-9]{3}$";

            // Act
            var resultKey = KeyGenerator.GenerateKey();

            Assert.False(string.IsNullOrWhiteSpace(resultKey));
            Assert.Matches(new Regex(PATTERN_CHECK), resultKey);
        }
    }
}
