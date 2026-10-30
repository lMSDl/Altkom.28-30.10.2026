using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TDD.Mock;

namespace TDD.Tests.Mock
{
    public class UserRegistrationServiceTests
    {
        [Fact]
        public void Register_EmailNotExists_AddsUser()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userRegistrationService = new UserRegistrationService(userRepositoryMock.Object, passwordHasherMock.Object);
            var fixture = new Fixture();
            var email = fixture.Create<string>();
            var password = fixture.Create<string>();
            var hashedPassword = fixture.Create<string>();

            passwordHasherMock.Setup(x => x.Hash(password)).Returns(hashedPassword);

            //Act
            userRegistrationService.Register(email, password);

            //Assert
            userRepositoryMock.Verify(x => x.Add(
                It.Is<User>(u => 
                    u.Email == email &&
                    u.PasswordHash == hashedPassword)),
                Times.Once);
        }

        [Fact]
        public void Register_EmailExists_InvalidOperationException()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var userRegistrationService = new UserRegistrationService(userRepositoryMock.Object, passwordHasherMock.Object);
            var fixture = new Fixture();
            var email = fixture.Create<string>();
            var password = fixture.Create<string>();

            userRepositoryMock.Setup(x => x.Exists(email)).Returns(true);

            //Act
            Action action = () => userRegistrationService.Register(email, password);

            //Assert
            Assert.Throws<InvalidOperationException>(action);
        }
    }
}
