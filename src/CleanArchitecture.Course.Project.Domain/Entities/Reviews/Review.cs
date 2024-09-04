using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Reviews.Events;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;

namespace CleanArchitecture.Course.Project.Domain.Entities.Reviews
{
    public sealed class Review : Entity<ReviewId>
    {
        private Review() { }
        private Review(
            ReviewId id,
            VehiculoId vehiculoId,
            AlquilerId alquilerId,
            UserId userId,
            Rating? rating,
            Comentario? comentario,
            DateTime fechaCreacion
            ) : base(id)
        {
            VehiculoId = vehiculoId;
            AlquilerId = alquilerId;
            UserId = userId;
            Rating = rating;
            Comentario = comentario;
            FechaCreacion = fechaCreacion;
        }
        public VehiculoId? VehiculoId { get; private set; }
        public AlquilerId? AlquilerId { get; private set; }
        public UserId? UserId { get; private set; }
        public Rating? Rating { get; private set; }
        public Comentario? Comentario { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        public static Result<Review> Create(
            Alquiler alquiler,
            Rating? rating,
            Comentario? comentario,
            DateTime fechaCreacion
            )
        {
            if(alquiler.Status != AlquilerStatus.Completado)
            {
                return Result.Failure<Review>(ReviewErrors.NotEligible);
            }

            var review = new Review(
                ReviewId.New(),
                alquiler.VehiculoId!,
                alquiler.Id!,
                alquiler.UserId!,
                rating,
                comentario,
                fechaCreacion
            );

            review.AddDomainEvent(new ReviewCreatedDomainEvent(review.Id!));
            
            return review;
        }
    }
}