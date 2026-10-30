namespace TDD.Tests
{
    public class StringCalculatorTests
    {
        /*[Fact]
        public void Add_EmptyString_Zero()
        {
            // Arrange
            var calculator = new StringCalculator();
            // Act
            var result = calculator.Add(string.Empty);
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Add_SingleNumber_SameNumber()
        {
            // Arrange
            var calculator = new StringCalculator();
            var input = 5;
            // Act
            var result = calculator.Add(input.ToString());
            // Assert
            Assert.Equal(input, result);
        }*/

        [Theory]
        [InlineData("", 0)]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("5", 5)]
        public void Add_SingleInput_SameNumber(string input, int expected)
        {
            // Arrange
            var calculator = new StringCalculator();
            // Act
            var result = calculator.Add(input);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(int.MinValue)]
        public void Add_NegativeNumber_ArgumentException(int input)
        {
            // Arrange
            var calculator = new StringCalculator();
            // Act
            Action action = () => calculator.Add(input.ToString());
            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData("1,2,3", 6)]
        [InlineData("4,5,6", 15)]
        public void Add_MultipleNumbers_SumOfNumbers(string input, int expected)
        {
            // Arrange
            var calculator = new StringCalculator();
            // Act
            var result = calculator.Add(input);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1,a,2,3", 6)]
        [InlineData("a", 0)]
        [InlineData("2.2", 0)]
        public void Add_NotANumber_Ignore(string input, int expected)
        {
            // Arrange
            var calculator = new StringCalculator();
            // Act
            var result = calculator.Add(input);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
