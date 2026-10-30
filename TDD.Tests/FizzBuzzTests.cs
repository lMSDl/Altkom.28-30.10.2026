namespace TDD.Tests
{
    public class FizzBuzzTests
    {
        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(9)]
        public void GetFizzBuzz_NumberDividedBy3_Fizz(int number)
        {
            // Arrange
            // Act
            var result = FizzBuzz.GetFizzBuzz(number);
            // Assert
            Assert.Equal("Fizz", result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        public void GetFizzBuzz_NumberDividedBy5_Buzz(int number)
        {
            // Arrange
            // Act
            var result = FizzBuzz.GetFizzBuzz(number);
            // Assert
            Assert.Equal("Buzz", result);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(30)]
        [InlineData(45)]
        public void GetFizzBuzz_NumberDividedBy3And5_FizzBuzz(int number)
        {
            // Arrange
            // Act
            var result = FizzBuzz.GetFizzBuzz(number);
            // Assert
            Assert.Equal("FizzBuzz", result);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        public void GetFizzBuzz_NumberNotDividedBy3Or5_IndexNumber(int number)
        {
            // Arrange
            // Act
            var result = FizzBuzz.GetFizzBuzz(number);
            // Assert
            Assert.Equal(number.ToString(), result);
        }

        [Theory]
        [InlineData(1, new [] {"1"})]
        [InlineData(2, new [] { "1", "2" })]
        [InlineData(15, new [] {"1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz"})]
        public void GetSequence_ReturnsCorrectSequence(int count, string[] expected)
        {
            // Arrange
            // Act
            var result = FizzBuzz.GetSequence(count);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
