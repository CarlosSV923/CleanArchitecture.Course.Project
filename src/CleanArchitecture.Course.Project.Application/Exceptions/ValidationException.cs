
namespace CleanArchitecture.Course.Project.Application.Exceptions
{
    public sealed class ValidationException(IEnumerable<ValidateError> errors) : Exception("Validation error")
    {
        public IEnumerable<ValidateError> Errors { get; private set; } = errors;
    }
}