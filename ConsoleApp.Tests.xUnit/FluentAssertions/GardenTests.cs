using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ConsoleApp.Tests.xUnit.FluentAssertions
{
    public class GardenTests
    {

        [Fact]
        public void Plant_ValidName_True()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 2;
            const string VALID_NAME = "a";
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            var result = garden.Plant(VALID_NAME);
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Plant_InvalidName_False()
        {
            // Arrange
            const int VALID_SIZE = 2;
            string validName = new Fixture().Create<string>();
            Garden garden = new Garden(VALID_SIZE);
            garden.Plant(validName);
            garden.Plant(validName);
            // Act
            var result = garden.Plant(validName);
            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Plant_DuplicatedName_CounterAddedToName()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + "2";

            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(duplicatedName);

            // Act
            garden.Plant(duplicatedName);

            // Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Plant_WhenDuplicatedItem_ThenCounterAddedToName(int numberOfCopies)
        {
            // Arrange
            int expectedCounter = numberOfCopies + 1;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + expectedCounter;

            Garden garden = new Garden(expectedCounter);
            Enumerable.Repeat(duplicatedName, numberOfCopies)
                .ToList()
                .ForEach(name => garden.Plant(name));

            // Act
            garden.Plant(duplicatedName);

            // Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Theory]
        [InlineData("sunflower", "sun")]
        [InlineData("róża czerwona", "róża")]
        public void Plant_NameIsSubstringOfAnotherName_DuplicationCounterNotAdded(string arrangeName, string actName)
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 3;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(actName);
            garden.Plant(arrangeName);
            string expectedName = actName + "2";

            // Act
            garden.Plant(actName);

            // Assert
            garden.GetItems().Should().Contain(expectedName);
        }

        [Fact]
        public void Plant_NullName_ThrowsArgumentNullException()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string? nullName = null;

            // Act
            Action action = () => garden.Plant(nullName);

            // Assert
            AssertException<ArgumentNullException>(action, "item");
        }

        [Fact]
        public void Plant_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string emptyName = "";
            // Act
            Action action = () => garden.Plant(emptyName);
            // Assert
            AssertException<ArgumentException>(action, "item", "Item cannot be empty or whitespace.");
        }

        //metoda pomocnicza do sprawdzania wyjątków ArgumentException i jego pochodnych
        private static void AssertException<T>(Action action, string expectedParamName, string? expectedMessage = null) where T : ArgumentException
        {
            using (new AssertionScope())
            {
                action.Should().Throw<T>()
                    .Where(ex => ex.ParamName == expectedParamName);
                if (expectedMessage != null)
                    action.Should().Throw<T>()
                        .Where(ex => ex.Message.Contains(expectedMessage));
            }
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData("\t")]
        [InlineData("  \n  \t")]
        public void Plant_EmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedParamName = "item";
            // Act
            Action action = () => garden.Plant(invalidName);
            // Assert
            action.Should().Throw<ArgumentException>()
                .Where(ex => ex.ParamName == expectedParamName)
                .WithMessage("Item cannot be empty or whitespace." + "*"); //wildcard pattern
        }

        [Fact]
        public void Plant_ValidItem_AddsItemToGarden()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validName = new Fixture().Create<string>();
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            garden.Plant(validName);
            // Assert
            garden.GetItems().Should().Contain(validName);
        }

        [Fact]
        public void GetItems_ReturnsCopyOfGardenCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();
            //Act
            var items2 = garden.GetItems();

            //Assert
            items1.Should().NotBeSameAs(items2);
        }
    }
}
