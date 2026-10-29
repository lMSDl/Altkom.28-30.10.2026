namespace ConsoleApp.Tests.xUnit
{
    public class ProgramTests
    {
        [Fact]
        public void Main_ConsoleOuput_HelloWorld()
        {
            //Arrange
            var main = typeof(Program).Assembly.EntryPoint;
            const string EXPECTED_OUTPUT = "Hello, World!";

            TextWriter consoleWriter = Console.Out;
            TextWriter textWriter = new StringWriter();
            Console.SetOut(textWriter);


            //Act
            main.Invoke(null, [Array.Empty<string>()]);

            //Assert
            Assert.Equal(EXPECTED_OUTPUT, textWriter.ToString().Trim());
            Console.SetOut(consoleWriter);
        }
    }
}
