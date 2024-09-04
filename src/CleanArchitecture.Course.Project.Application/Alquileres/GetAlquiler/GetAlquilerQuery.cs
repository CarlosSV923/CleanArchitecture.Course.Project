using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;

namespace CleanArchitecture.Course.Project.Application.Alquileres.GetAlquiler
{
    public sealed record GetAlquilerQuery(Guid AlquilerId) : IQuery<AlquilerResponse> {
        
    }
}