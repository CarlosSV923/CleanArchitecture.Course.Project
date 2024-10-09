using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.ArchitectureTests.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace CleanArchitecture.Course.Project.ArchitectureTests.Application
{
    public class ApplicationTests : BaseTest
    {
        [Fact]
        public void CommandHandler_Should_NotBePublic()
        {
            // Arrange
            var commandHandlers = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .GetTypes();

            // Act
            var publicCommandHandlers = commandHandlers
                .Where(t => t.IsPublic);

            // Assert
            publicCommandHandlers.Should().BeEmpty();
        }

        [Fact]
        public void QueryHandler_Should_NotBePublic()
        {
            // Arrange
            var queryHandlers = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .GetTypes();

            // Act
            var publicQueryHandlers = queryHandlers
                .Where(t => t.IsPublic);

            // Assert
            publicQueryHandlers.Should().BeEmpty();
        }
    }
}