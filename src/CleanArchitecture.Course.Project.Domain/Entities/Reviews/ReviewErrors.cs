using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Reviews
{
    public static class ReviewErrors
    {
        public static Error NotEligible => new("Review.NotEligible", "Este review y calificacion para el auto no es elegible porque aun no se completa.");
    }
}