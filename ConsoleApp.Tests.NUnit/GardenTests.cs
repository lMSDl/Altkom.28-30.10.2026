using AutoFixture;

namespace ConsoleApp.Tests.NUnit
{
    public class GardenTests
    {
        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void TearDown()
        {
        }


        [Test]
        public void Plant_ValidName_True()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 2;
            const string VALID_NAME = "a";
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            var result = garden.Plant(VALID_NAME);
            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
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
            Assert.That(result, Is.False);
        }

        [Test]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        //[Theory]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        [TestCase("sunflower", "sun")]
        [TestCase("róża czerwona", "róża")]
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
            Assert.That(garden.GetItems(), Does.Contain(expectedName));
        }

        [Test]
        public void Plant_NullName_ThrowsArgumentNullException()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string? nullName = null;

            // Act
            TestDelegate action = () => garden.Plant(nullName);

            // Assert
            AssertException<ArgumentNullException>(action, "item");
        }

        [Test]
        [Ignore("Test is ignored because it is redundant with Plant_EmptyOrWhitespaceName_ThrowsArgumentException")]
        public void Plant_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string emptyName = "";
            // Act
            TestDelegate action = () => garden.Plant(emptyName);
            // Assert
            AssertException<ArgumentException>(action, "item", "Item cannot be empty or whitespace.");
        }

        private static void AssertException<T>(TestDelegate action, string expectedParamName, string? expectedMessage = null) where T : ArgumentException
        {
            var argumentException = Assert.Throws<T>(action);
            Assert.That(argumentException.ParamName, Is.EqualTo(expectedParamName));
            if (expectedMessage is not null)
                Assert.That(argumentException.Message, Is.EqualTo(expectedMessage));
        }

        public static IEnumerable<object[]> GetInvalidName()
        {
            yield return new object[] { "" };
            yield return new object[] { " " };
            yield return new object[] { " " }; //non-breaking space
            yield return new object[] { "    " };
            yield return new object[] { "\t" };
            yield return new object[] { "  \n  \t" };
        }


        /*[TestCase("")]
        [TestCase(" ")]
        [TestCase(" ")]
        [TestCase("    ")]
        [TestCase("\t")]
        [TestCase("  \n  \t")]*/
        //[TestCaseSource(nameof(GetInvalidName))]            
        [TestCaseSource(typeof(GardenInvalidNames))]
        public void Plant_EmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedParamName = "item";
            // Act & Assert
            ArgumentException argumentException = Assert.Catch<ArgumentException>(() => garden.Plant(invalidName));
            Assert.That(argumentException.ParamName, Is.EqualTo(expectedParamName));
        }

        [Test]
        public void Plant_ValidItem_AddsItemToGarden()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validName = new Fixture().Create<string>();
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            garden.Plant(validName);
            // Assert
            Assert.That(garden.GetItems(), Does.Contain(validName));
        }

        [Test]
        public void GetItems_ReturnsCopyOfGardenCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();
            //Act
            var items2 = garden.GetItems();

            //Assert
            Assert.That(items1, Is.Not.SameAs(items2));
        }
    }
}
