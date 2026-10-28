using AutoFixture;

namespace ConsoleApp.Tests.xUnit
{
    public class GardenTests : IDisposable
    {
        //używanie metod setup i teardown w testach jednostkowych jest niezalecane,
        //ponieważ testy powinny być niezależne i nie powinny mieć efektów ubocznych.
        //metody setup i teardown są wywoływane przed i po KAŻDYM teście
        //Zamiast tego możemy używać metod pomocniczych (np. "Garden CreateEmptyGarden()" lub "Garden CreateGarden(int minmalValidSize)")

        //xUnit nie posiada metod setup, ale możemy użyć konstruktora klasy testowej
        public GardenTests()
        {
        }
        // xUnit nie posiada metod teardown, ale możemy użyć metody Dispose
        public void Dispose()
        {
        }


        [Fact]
        //Should_ExpectedBehavior_When_StateUnderTest
        //MethodName_StateUnderTest_ExpectedBehavior
        //Given_When_Then: Given_Preconditions_When_Action_Then_ExpectedResult (stosowane w BDD)
        //public void Plant_GivesTrue_WhenNameIsValid()
        //public void Plant_PassValidName_ReturnsTrue()
        public void Plant_ValidName_True()
        {
            // Arrange
            //opisujemy intencje
            //nie używamy "magicznych" wartości, tylko takie, które mają znaczenie dla testu
            const int MINIMAL_VALID_SIZE = 2; //używamy parametru, który daje minimalny ale wystarczający rozmiar ogrodu, aby test mógł się wykonać
            const string VALID_NAME = "a"; //używamy parametru z minimalnym przekazem (ograniczamy możliwość nadinterpretacji)
            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            // Act
            var result = garden.Plant(VALID_NAME);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Plant_InvalidName_False()
        {
            // Arrange
            const int VALID_SIZE = 2;
            string validName = new Fixture().Create<string>(); //używamy biblioteki AutoFixture do generowania losowych danych testowych, aby uniknąć "magicznych" wartości
            Garden garden = new Garden(VALID_SIZE);
            garden.Plant(validName);
            garden.Plant(validName); // Plantujemy roślinę o losowej nazwie, aby przygotować ogród do testu
            // Act
            var result = garden.Plant(validName);
            // Assert
            Assert.False(result);
        }

        [Fact]
        //public void Plant_AddsCounterToName_WhenDuplicatedItem()
        //public void Plant_WhenDuplicatedItem_ThenCounterAddedToName()
        public void Plant_DuplicatedName_CounterAddedToName()
        {
            // Arrange
            const int MINIMAL_VALID_SIZE = 2;
            string duplicatedName = new Fixture().Create<string>();
            string expectedName = duplicatedName + "2"; //oczekujemy, że przy dodaniu rośliny o tej samej nazwie, zostanie dodany licznik do nazwy

            Garden garden = new Garden(MINIMAL_VALID_SIZE);
            garden.Plant(duplicatedName);

            // Act
            garden.Plant(duplicatedName);

            // Assert
            Assert.Contains(expectedName, garden.GetItems());
            //powinniśmy unikać asercji wielu rzeczy w jednym teście
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
            //var argumentNullException = Assert.Throws<ArgumentNullException>(action); //sprawdzenie, czy metoda Plant rzuca wyjątek ArgumentNullException
            //Assert.ThrowsAny<ArgumentException>(action); //sprawdzenie czy metoda Plant rzuca wyjątek ArgumentException lub jego pochodną (dziedziczącą po ArgumentException)
            //Assert.Contains("item", argumentNullException.ParamName); //sprawdzenie, czy parametr "item" jest tym, który spowodował wyjątek
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
            //mimo wielu Assertów w tym teście, test jest nadal zgodny z dobrymi praktykami, ponieważ sprawdzamy tylko jeden przypadek (wyjątek ArgumentException)
            //var argumentException = Assert.Throws<ArgumentException>(action);
            //Assert.Contains("item", argumentException.ParamName);
            //Assert.Contains("Item cannot be empty or whitespace.", argumentException.Message);
            AssertException<ArgumentException>(action, "item", "Item cannot be empty or whitespace.");
        }

        //metoda pomocnicza do sprawdzania wyjątków ArgumentException i jego pochodnych
        private static void AssertException<T>(Action action, string expectedParamName, string? expectedMessage = null) where T : ArgumentException
        {
            var argumentException = Assert.Throws<T>(action);
            Assert.Contains(expectedParamName, argumentException.ParamName);
            if (expectedMessage is not null)
                Assert.Contains(expectedMessage, argumentException.Message);
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
            Assert.Contains(validName, garden.GetItems());
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
            Assert.NotSame(items1, items2); //sprawdzamy, czy GetItems zwraca kopię kolekcji, a nie referencję do tej samej kolekcji
        }
    }
}
