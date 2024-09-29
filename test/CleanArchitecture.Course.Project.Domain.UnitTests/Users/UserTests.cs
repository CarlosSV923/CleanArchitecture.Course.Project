using CleanArchitecture.Course.Project.Domain.Entities.Roles;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Users.Events;
using CleanArchitecture.Course.Project.Domain.UnitTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Users
{
    public class UserTests : BaseTest
    {
        [Fact]
        public void Create_Should_SetPropertyValues()
        {
            // Arrange --> Creacion mock de datos
            // Se crea UserMock para tener datos de prueba

            // Act --> Ejecucion de la prueba
            var user = User.Create(
                                UserMock.Name,
                                UserMock.LastName,
                                UserMock.Email,
                                UserMock.Password
                                );

            // Assert --> Verificacion de los resultados

            user.Nombre.Should().Be(UserMock.Name);
            user.Apellido.Should().Be(UserMock.LastName);
            user.Email.Should().Be(UserMock.Email);
            user.PasswordHash.Should().Be(UserMock.Password);
        }

        [Fact]
        public void Create_Should_RaiseUserDomainEvent()
        {
            var user = User.Create(
                                UserMock.Name,
                                UserMock.LastName,
                                UserMock.Email,
                                UserMock.Password
                                );
            
            user.GetDomainEvents().Should().NotBeEmpty();
            // var domainEvent = user.GetDomainEvents().OfType<UserCreatedDomainEvent>().SingleOrDefault();
            var domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);
            domainEvent!.UserId.Should().Be(user.Id);
        }

        [Fact]
        public void Create_Should_AddRegistrerRoleToUser() {
            var user = User.Create(
                                UserMock.Name,
                                UserMock.LastName,
                                UserMock.Email,
                                UserMock.Password
                                );

            user.Roles.Should().NotBeEmpty();
            user.Roles.Should().Contain(Role.Cliente);
        }

    }

}