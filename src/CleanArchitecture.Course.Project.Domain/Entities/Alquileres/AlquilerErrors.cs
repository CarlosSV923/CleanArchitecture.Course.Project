
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Alquileres
{
    public static class AlquilerErrors
    {
        public static Error NotFound => new("Alquiler.Found", "No se encontró el alquiler solicitado");
        public static Error Overlap => new("Alquiler.Overlap", "El alquiler esta en conflicto con otro");
        public static Error NotReserved => new("Alquiler.NotReserved", "El alquiler no esta reservado");
        public static Error NotConfirmed => new("Alquiler.NotConfirmed", "El alquiler no esta confirmado");
        public static Error AlreadyStarted => new("Alquiler.AlreadyStarted", "El alquiler ya ha comenzado");
    }

}