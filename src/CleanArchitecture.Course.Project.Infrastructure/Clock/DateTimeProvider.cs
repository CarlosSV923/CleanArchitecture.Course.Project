using CleanArchitecture.Course.Project.Application.Abstractions.Clock;

namespace CleanArchitecture.Course.Project.Infrastructure.Clock
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentTime => DateTime.UtcNow;
    }
}