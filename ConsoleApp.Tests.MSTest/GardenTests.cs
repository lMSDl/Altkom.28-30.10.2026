using AutoFixture;

namespace ConsoleApp.Tests.MSTest
{
    [TestClass]
    public class GardenTests
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestCleanup]
        public void TearDown()
        {
        }


        [TestMethod]
        public void Plant_ValidName_True()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 2;
            const string VALID_NAME = "a";
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            var result = garden.Plant(VALID_NAME);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
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
            Assert.IsFalse(result);
        }

        [TestMethod]
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
            Assert.Contains(expectedName, garden.GetItems());
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(5)]
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
            Assert.Contains(expectedName, garden.GetItems());
        }

        [TestMethod]
        [DataRow("sunflower", "sun")]
        [DataRow("róża czerwona", "róża")]
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
            Assert.Contains(expectedName, garden.GetItems());
        }

        [TestMethod]
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

        [TestMethod]
        [Ignore("Replaced by Plant_EmptyOrWhitespaceName_ThrowsArgumentException")]
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

        private static void AssertException<T>(Action action, string expectedParamName, string? expectedMessage = null) where T : ArgumentException
        {
            var argumentException = Assert.Throws<T>(action);
            Assert.Contains(expectedParamName, argumentException.ParamName);
            if (expectedMessage is not null)
                Assert.Contains(expectedMessage, argumentException.Message);
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


        [TestMethod]
        /*[DataRow("")]
        [DataRow(" ")]
        [DataRow(" ")]
        [DataRow("    ")]
        [DataRow("\t")]
        [DataRow("  \n  \t")]*/
        //[DynamicData(nameof(GetInvalidName))]
        [DynamicData(nameof(GardenInvalidNames.Data), typeof(GardenInvalidNames))]
        public void Plant_EmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            string expectedParamName = "item";
            // Act
            Action action = () => garden.Plant(invalidName);
            // Assert
            AssertException<ArgumentException>(action, expectedParamName, "Item cannot be empty or whitespace.");
        }

        [TestMethod]
        public void Plant_ValidItem_AddsItemToGarden()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 1;
            string validName = new Fixture().Create<string>();
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            garden.Plant(validName);
            // Assert
            Assert.Contains(validName, garden.GetItems());
        }

        [TestMethod]
        public void GetItems_ReturnsCopyOfGardenCollection()
        {
            //Arrange
            const int MINIMAL_VALID_SIZE = 0;
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            var items1 = garden.GetItems();
            //Act
            var items2 = garden.GetItems();

            //Assert
            Assert.AreNotSame(items1, items2);
        }
    }
}
