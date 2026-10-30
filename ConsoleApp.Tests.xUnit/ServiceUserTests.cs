using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Tests.xUnit
{
    public class ServiceUserTests
    {
        [Fact]
        public void SetServiceFriendlyName_ShoudSetName()
        {
            //Arrange
            var service = new Mock<IService>();
            var serviceUser = new ServiceUser(service.Object);

            var fixture = new Fixture();
            string expectedName = fixture.Create<string>();
            
            //service.SetupProperty(s => s.Name);

            // Act
            serviceUser.SetServiceFriendlyName(expectedName);

            // Assert
            service.VerifySet(s => s.Name = expectedName, Times.Once);
            //Assert.Equal(expectedName, service.Object.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ValidateUniqueId_UniqueIdIsNullOrEmpty_False(string? uniqueId)
        {
            //Arrange
            var service = new Mock<IService>();
            var serviceUser = new ServiceUser(service.Object);

            service.SetupGet(x => x.UniqueId).Returns(uniqueId);

            // Act
            var result = serviceUser.ValidateUniqueId();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateUniqueId_UniqueIdIsValidGuid_True()
        {
            //Arrange
            var service = new Mock<IService>();
            var serviceUser = new ServiceUser(service.Object);

            string validGuid = Guid.NewGuid().ToString();
            service.SetupGet(x => x.UniqueId).Returns(validGuid);

            // Act
            var result = serviceUser.ValidateUniqueId();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateUniqueId_UniqueIdIsNotValidGuid_False()
        {
            //Arrange
            var service = new Mock<IService>();
            var serviceUser = new ServiceUser(service.Object);

            string invalidGuid = "invalid-guid";
            service.SetupGet(x => x.UniqueId).Returns(invalidGuid);

            // Act
            var result = serviceUser.ValidateUniqueId();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StartService_SetsIsServiceStartedToTrue()
        {
            // Arrange
            var service = new Mock<IService>();
            var serviceUser = new ServiceUser(service.Object);

            // Act
            serviceUser.StartService();
            service.Raise(s => s.OnServiceStarted += default, EventArgs.Empty);

            // Assert
            service.Verify(x => x.StartService(), Times.Once);
            Assert.True(serviceUser.IsServiceStarted);
        }
    }
}
