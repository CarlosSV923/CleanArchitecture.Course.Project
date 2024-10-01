using System.Runtime.CompilerServices;
using CleanArchitecture.Course.Project.Application.IntegrationTests.Infrastructure;
using CleanArchitecture.Course.Project.Application.Vehiculos.SearchVehiculos;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Course.Project.Application.IntegrationTests.Vehiculos
{
    public class SearchVehiculosTests : BaseIntegrationTest
    {
        public SearchVehiculosTests(IntegrationTestsWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SearchVehiculos_Should_ReturnEmptyList_WhenDateRangeInvalid()
        {
            // Arrange
            var request = new SearchVehiculosQuery(
                new DateOnly(2023, 1, 1),
                new DateOnly(2022, 1, 1)
            );

            // Act
            var response = await _sender.Send(request);

            // Assert
            response.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchVehiculos_Should_ReturnVehiculos_WhenDateRangeValid()
        {
            // Arrange
            var request = new SearchVehiculosQuery(
                new DateOnly(2023, 1, 1),
                new DateOnly(2026, 1, 1)
            );

            // Act
            var response = await _sender.Send(request);

            // Assert
            response.Value.Should().NotBeEmpty();
        }
    }
}