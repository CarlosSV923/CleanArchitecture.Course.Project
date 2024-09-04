namespace CleanArchitecture.Course.Project.Application.Abstractions.Clock
{
    public interface IDateTimeProvider
    {
        DateTime CurrentTime { get; }
    }
}