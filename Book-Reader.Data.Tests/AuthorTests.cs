using Book_Reader.Data.Models;
using Xunit;

namespace Book_Reader.Data.Tests
{
    public class AuthorTests
    {
        [Theory]
        [InlineData("Peter", "F", "Hamilton", "Peter F Hamilton")]
        [InlineData("Peter", " ", "Hamilton", "Peter Hamilton")]
        [InlineData("Peter", "", "Hamilton", "Peter Hamilton")]
        [InlineData("Peter", null, "Hamilton", "Peter Hamilton")]
        [InlineData(" ", " ", "Hamilton", "Hamilton")]
        [InlineData("", "", "Hamilton", "Hamilton")]
        [InlineData(null, null, "Hamilton", "Hamilton")]
        public void Author_FullName(string firstName, string middleName, string lastName, string expectedFullName)
        {
            // Arrange
            var author = new Author(firstName, middleName, lastName);

            // Act
            var authorsFullName = author.FullName;

            // Assert
            Assert.Equal(expectedFullName, authorsFullName);


        }
    }
}
