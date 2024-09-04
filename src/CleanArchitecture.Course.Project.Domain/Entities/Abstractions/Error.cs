namespace CleanArchitecture.Course.Project.Domain.Entities.Abstractions
{
    public record Error(string Code, string Message) {
        public static Error None => new(string.Empty, string.Empty);

        public static Error NullValue => new("Error.NullValue", "El valor no puede ser nulo");
    }
}