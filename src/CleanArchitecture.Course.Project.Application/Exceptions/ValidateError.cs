namespace CleanArchitecture.Course.Project.Application.Exceptions
{
    public sealed record ValidateError(string PropertyName, string ErrorMessage);
}
