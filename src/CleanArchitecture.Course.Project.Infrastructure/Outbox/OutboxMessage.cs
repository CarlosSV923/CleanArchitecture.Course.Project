namespace CleanArchitecture.Course.Project.Infrastructure.Outbox
{
    public sealed class OutboxMessage(Guid id, string type, string data, DateTime createdAt)
    {
        public Guid Id { get; init; } = id;
        public string? Type { get; init; } = type;
        public string? Data { get; init; } = data;
        public DateTime CreatedAt { get; init; } = createdAt;
        public DateTime? ProcessedAt { get; init; }
        public string? Error { get; init; }

    }
}