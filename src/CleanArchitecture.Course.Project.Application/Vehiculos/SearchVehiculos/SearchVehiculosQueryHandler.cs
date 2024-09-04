using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using Dapper;

namespace CleanArchitecture.Course.Project.Application.Vehiculos.SearchVehiculos
{
    internal sealed class SearchVehiculosQueryHandler(ISqlConnectionFactory sqlConectionFactory) : IQueryHandler<SearchVehiculosQuery, IReadOnlyList<VehiculoResponse>>
    {   
        private static readonly int[] ActiveAlquilerStatus = {
            
            (int)AlquilerStatus.Confirmado,
            (int)AlquilerStatus.Reservado,
            (int)AlquilerStatus.Completado
        };
        private readonly ISqlConnectionFactory _sqlConectionFactory = sqlConectionFactory;

        public async Task<Result<IReadOnlyList<VehiculoResponse>>> Handle(SearchVehiculosQuery request, CancellationToken cancellationToken)
        {
            if(request.FechaInicio > request.FechaFin)
            {
                return new List<VehiculoResponse>();
            }

            using var connection = _sqlConectionFactory.CreateConnection();

            var sql = """
                SELECT
                    a.id AS Id,
                    a.vin AS Vin,
                    a.modelo AS Modelo,
                    a.tipo AS Tipo,
                    a.precio_monto AS Precio,
                    a.precio_tipo_moneda AS TipoMoneda,
                    a.fecha_creacion AS FechaCreacion,
                    a.direccion_pais AS Pais,
                    a.direccion_ciudad AS Ciudad,
                    a.direccion_calle AS Calle,
                    a.direccion_estado AS Estado,
                    a.direccion_departamento AS Departamento,
                FROM vehiculos AS a
                WHERE NOT EXIST(
                    SELECT 1
                    FROM alquileres AS b
                    WHERE b.vehiculo_id = a.id
                    AND b.fecha_inicio <= @FechaFin
                    AND b.fecha_fin >= @FechaInicio
                    AND b.status = ANY(@ActiveAlquilerStatus)
                )

            """;

            var vehiculos = await connection.QueryAsync<VehiculoResponse, DireccionResponse, VehiculoResponse>(
                sql,
                (vehiculo, direccion) => {
                    vehiculo.Direccion = direccion;
                    return vehiculo;
                },
                new {
                    request.FechaInicio,
                    request.FechaFin,
                    ActiveAlquilerStatus
                
                },
                splitOn: "Pais"
                
            );

            return vehiculos.ToList();
        }
    }
}