using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Reviews
{
    public sealed record Rating
    {
        public static readonly Error Invalid = new("Rating.Invalid", "El valor de la calificacion debe estar entre 1 y 5.");
        public int Value { get; }

        private Rating(int value)
        {
            Value = value;
        }

        public static Result<Rating> Create(int value)
        {
            if (value < 1 || value > 5)
            {
                return Result.Failure<Rating>(Invalid);
            }

            return new Rating(value);
        }
    }
}