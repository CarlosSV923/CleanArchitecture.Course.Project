using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Vehiculos.Specifications
{
    public class VehiculoPaginationSpecification : BaseSpecification<Vehiculo, VehiculoId>
    {
        public VehiculoPaginationSpecification(
            string? sort,
            int pageSize,
            int pageIndex,
            string? search
        ) : base(
            x => string.IsNullOrEmpty(search) || x.Modelo == new Modelo(search)
        )
        {
            ApplyPaging(pageSize * (pageIndex - 1), pageSize);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "modeloAsc":
                        AddOrderBy(x => x.Modelo!);
                        break;
                    case "modeloDesc":
                        AddOrderByDesc(x => x.Modelo!);
                        break;
                    default:
                        AddOrderBy(x => x.FechaUltimoAlquiler!);
                        break;
                }
            }
            else
            {
                AddOrderBy(x => x.FechaUltimoAlquiler!);
            }
        }
    }
}