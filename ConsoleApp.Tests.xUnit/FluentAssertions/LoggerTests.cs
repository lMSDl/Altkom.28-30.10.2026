using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ConsoleApp.Tests.xUnit.FluentAssertions
{
    public class LoggerTests
    {
       
        [Fact]
        public void Log_AnyInput_CorrectEventArgs_V1()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            object? sender = null;
            Logger.LoggerEventArgs? eventArgs = null;
            logger.MessageLogged += (s, e) => { sender = s; eventArgs = e; };

            //Act
            logger.Log(log);

            //Assert
            using (new AssertionScope())
            {
                sender.Should().Be(logger);
                eventArgs.Should().NotBeNull();
                eventArgs.Message.Should().Be(log);
                eventArgs.Timestamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(100));
            }

        }

        [Fact]
        public void Log_AnyInput_CorrectEventArgs_V2()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            using var monitor = logger.Monitor();

            //Act
            logger.Log(log);

            //Assert
            using (new AssertionScope())
            {
                monitor.Should().Raise(nameof(Logger.MessageLogged))
                .WithSender(logger)
                .WithArgs<Logger.LoggerEventArgs>(e => e.Message.Equals(log));
            }
        }
    }
}
