using AutoFixture;
using System.Globalization;

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

        [Fact]
        public async Task GetLogsAsync_ValidDateRange_LoggedMessage()
        {
            //Arrange
            const string LOG_SPLITTER = ": ";
            const int LOG_SPLITTER_COUNT = 2;
            const string EXPECTED_DATE_FORMAT = "dd.MM.yyyy HH:mm";
            var logger = new Logger();
            var fixture = new Fixture();
            var expectedLog = fixture.Create<string>();

            logger.Log(fixture.Create<string>());
            var dateTimeStart = DateTime.Now;
            logger.Log(expectedLog);
            var dateTimeEnd = DateTime.Now;
            logger.Log(fixture.Create<string>());

            //Act
            var result = await logger.GetLogsAsync(dateTimeStart, dateTimeEnd);

            //Assert
            //wersja prostrza
            Assert.Contains(expectedLog, result);

            //wersja pełna
            //data czas: wiadomosc
            var splittedResult = result.Split(LOG_SPLITTER);
            Assert.Equal(LOG_SPLITTER_COUNT, splittedResult.Length);
            Assert.Equal(expectedLog, splittedResult[1]);
            Assert.True(DateTime.TryParseExact(splittedResult[0], EXPECTED_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }
    }
}
