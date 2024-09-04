using Bogus;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Infrastructure;
using Dapper;

namespace CleanArchitecture.Course.Project.Api.Extensions
{
    public static class SeedDataExtension
    {

        public static void SeedAuthData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider;

            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();

                if (!context.Set<User>().Any())
                {

                    var hashPassword = BCrypt.Net.BCrypt.HashPassword("Test1234");

                    var user = User.Create(
                        new Name("Carlos"),
                        new LastName("Perez"),
                        new Email("csesme@gmail.com"),
                        new PasswordHash(hashPassword)
                    );

                    context.Add(user);

                    var hashPassword2 = BCrypt.Net.BCrypt.HashPassword("AdminTest1234");

                    var userAdmin = User.Create(
                        new Name("Admin"),
                        new LastName("Admin"),
                        new Email("admin@gmail.com"),
                        new PasswordHash(hashPassword2)
                    );

                    context.Add(userAdmin);

                    context.SaveChangesAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

            using var connection = sqlConnectionFactory.CreateConnection();

            var faker = new Faker("es");

            List<Object> vehiculos = [];
            for (int i = 0; i < 100; i++)
            {
                var vehiculo = new
                {
                    Id = Guid.NewGuid(),
                    Modelo = faker.Vehicle.Model(),
                    Vin = faker.Vehicle.Vin(),
                    Pais = faker.Address.Country(),
                    Ciudad = faker.Address.City(),
                    Departamento = faker.Address.State(),
                    Provincia = faker.Address.StateAbbr(),
                    Calle = faker.Address.StreetAddress(),
                    Direccion = faker.Address.FullAddress(),
                    Precio = faker.Random.Decimal(10000, 50000),
                    PrecioTipoMoneda = "USD",
                    PrecioMantenimiento = faker.Random.Decimal(1000, 5000),
                    Mantenimiento = faker.Random.Decimal(1000, 5000),
                    FechaUltimoAlquiler = faker.Date.Past(),
                };
                vehiculos.Add(vehiculo);
            }

            var sql = "INSERT INTO vehiculos (id, modelo, vin, pais, ciudad, departamento, provincia, calle, direccion, precio, precio_tipo_moneda, precio_mantenimiento, mantenimiento, fecha_ultimo_alquiler) VALUES (@Id, @Modelo, @Vin, @Pais, @Ciudad, @Departamento, @Provincia, @Calle, @Direccion, @Precio, @PrecioTipoMoneda, @PrecioMantenimiento, @Mantenimiento, @FechaUltimoAlquiler)";
            connection.Execute(sql, vehiculos);

        }
    }
}
