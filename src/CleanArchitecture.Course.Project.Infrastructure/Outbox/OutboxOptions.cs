namespace CleanArchitecture.Course.Project.Infrastructure.Outbox
{
    public class OutboxOptions
    {
        public int IntervalInSeconds { get; init; }

        public int BatchSize { get; init; }

        public bool Enabled { get; init; }
    }
}