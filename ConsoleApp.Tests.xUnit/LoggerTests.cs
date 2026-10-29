using AutoFixture;

namespace ConsoleApp.Tests.xUnit
{
    public class LoggerTests
    {
        [Fact]
        public void Log_AnyInput_EventIvokedOnce()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            var eventInvoked = 0;
            const int EXPECTED_EVENT_INVOKED = 1;
            logger.MessageLogged += (_, _) => eventInvoked++;

            //Act
            logger.Log(log);

            //Assert
            Assert.Equal(EXPECTED_EVENT_INVOKED, eventInvoked);
        }

        [Fact]
        public void Log_AnyInput_CorrectEventArgs()
        {
            //Arrange
            var logger = new Logger();
            var log = new Fixture().Create<string>();
            object? sender = null;
            Logger.LoggerEventArgs? eventArgs = null;
            logger.MessageLogged += (s, e) => { sender = s; eventArgs = e; };

            //Act
            var dateTimeStart = DateTime.Now;
            logger.Log(log);
            var dateTimeEnd = DateTime.Now;

            //Assert
            Assert.Equal(logger, sender);
            Assert.NotNull(eventArgs);
            Assert.Equal(log, eventArgs.Message);
            Assert.InRange(eventArgs.Timestamp, dateTimeStart, dateTimeEnd);
        }
    }
}
