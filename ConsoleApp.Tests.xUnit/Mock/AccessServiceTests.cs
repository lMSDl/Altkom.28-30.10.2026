using AutoFixture;
using ConsoleApp.Mock;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Tests.xUnit.Mock
{
    public class AccessServiceTests
    {
        [Fact]
        public void CanAccessAdminPanel_NotExistingUser_False()
        {
            // Arrange
            const int NON_EXISTING_USER_ID = 1;
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((User?)null);

            var accessService = new AccessService(userRepositoryMock.Object);
            
            // Act
            var result = accessService.CanAccessAdminPanel(NON_EXISTING_USER_ID);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanAccessAdminPanel_NotActiveUser_False()
        {
            // Arrange
            var fixture = new Fixture();
            int existingUserId = fixture.Create<int>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var user = fixture.Build<User>().With(u => u.IsActive, false).Create();

            userRepositoryMock.Setup(repo => repo.GetById(existingUserId)).Returns(user).Verifiable();

            var accessService = new AccessService(userRepositoryMock.Object);

            // Act
            var result = accessService.CanAccessAdminPanel(existingUserId);

            // Assert
            Assert.False(result);
            userRepositoryMock.VerifyAll();
        }

        [Fact]
        public void CanAccessAdminPanel_NotAdmin_False()
        {
            // Arrange
            var fixture = new Fixture();
            int existingUserId = fixture.Create<int>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var user = fixture.Build<User>().With(u => u.IsActive, true).Create();

            userRepositoryMock.Setup(repo => repo.GetById(existingUserId)).Returns(user);

            var accessService = new AccessService(userRepositoryMock.Object);

            // Act
            var result = accessService.CanAccessAdminPanel(existingUserId);

            // Assert
            Assert.False(result);
            userRepositoryMock.Verify(repo => repo.GetById(existingUserId), Times.Once);
        }

        [Fact]
        public void CanAccessAdminPanel_Admin_True()
        {
            // Arrange
            var fixture = new Fixture();
            int existingUserId = fixture.Create<int>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var user = fixture.Build<User>().With(u => u.IsActive, true).With(u => u.Role, "Admin").Create();

            userRepositoryMock.Setup(repo => repo.GetById(existingUserId)).Returns(user);

            var accessService = new AccessService(userRepositoryMock.Object);

            // Act
            var result = accessService.CanAccessAdminPanel(existingUserId);

            // Assert
            Assert.True(result);
            userRepositoryMock.Verify(repo => repo.GetById(existingUserId), Times.Once);
        }
    }
}
