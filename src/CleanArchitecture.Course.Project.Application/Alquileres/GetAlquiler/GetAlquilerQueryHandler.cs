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

            var sql = @"
            SELECT
                  id AS Id,
                  vehiculo_id AS VehiculoId,
                  user_id AS UserId,
                  status AS Status,
                  precio_por_periodo_monto AS PrecioAlquiler,
                  precio_por_periodo_tipo_moneda AS TipoMonedaAlquiler,
                  mantenimiento_monto AS PrecioMantenimiento,
                  mantenimiento_tipo_moneda AS TipoMonedaMantenimiento,
                  accesorios_monto AS AccesoriosPrecio,
                  accesorios_tipo_moneda AS TipoMonedaAccesorio,
                  precio_total_monto AS PrecioTotal,
                  precio_total_tipo_moneda AS PrecioTotalTipoMoneda,
                  duracion_inicio AS DuracionInicio,
                  duracion_fin AS DuracionFinal,
                  fecha_creacion AS FechaCreacion
             FROM alquileres WHERE id=@AlquilerId
            ";

            var alquiler = await connection.QueryFirstOrDefaultAsync<AlquilerResponse>(
                sql,
                new { request.AlquilerId }
            );

            return alquiler!;
        }
    }
}