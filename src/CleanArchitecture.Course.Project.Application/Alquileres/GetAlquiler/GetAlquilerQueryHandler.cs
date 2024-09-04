using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Dapper;

namespace CleanArchitecture.Course.Project.Application.Alquileres.GetAlquiler
{
    internal sealed class GetAlquilerQueryHandler(ISqlConnectionFactory sqlConectionFactory) : IQueryHandler<GetAlquilerQuery, AlquilerResponse>
    {
        private readonly ISqlConnectionFactory _sqlConectionFactory = sqlConectionFactory;

        public async Task<Result<AlquilerResponse>> Handle(GetAlquilerQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConectionFactory.CreateConnection();

            var sql = """
                SELECT 
                    id AS Id,
                    vehiculo_id AS VehiculoId,
                    user_id AS UserId,
                    status AS Status,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin,
                    fecha_creacion AS FechaCreacion
                    precio_por_periodo AS PrecioAlquiler
                    precio_por_periodo_tipo_moneda AS TipoMonedaAlquiler,
                    precio_manteniiento AS PrecioMantenimiento,
                    precio_manteniiento_tipo_moneda AS TipoMonedaMantenimiento
                    precio_accesorios AS PrecioAccesorios,
                    precio_accesorios_tipo_moneda AS TipoMonedaAccesorios
                    precio_total AS PrecioTotal,
                    precio_total_tipo_moneda AS TipoMonedaTotal
                FROM alquiler WHERE Id = @AlquilerId
            """;

            var alquiler = await connection.QueryFirstOrDefaultAsync<AlquilerResponse>(
                sql,
                new { request.AlquilerId }
            );

            return alquiler!;
        }
    }
}