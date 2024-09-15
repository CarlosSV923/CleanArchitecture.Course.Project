using System.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Clock;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace CleanArchitecture.Course.Project.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal sealed class InvokeOutboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<InvokeOutboxJob> logger
    ) : IJob
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
        private readonly IPublisher _publisher = publisher;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
        private readonly ILogger<InvokeOutboxJob> _logger = logger;

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executing Outbox job...");

            using var connection = _sqlConnectionFactory.CreateConnection();

            using var transaction = connection.BeginTransaction();

            var sql = $@"

                SELECT 
                    id,
                    data
                FROM outbox_messages
                WHERE processed_at IS NULL
                ORDER BY created_at
                LIMIT {_outboxOptions.BatchSize}
                FOR UPDATE SKIP LOCKED
            ";

            var outboxMessages = (await connection.QueryAsync<OutboxMessageData>(sql, transaction: transaction)).ToList();

            foreach (
                var outboxMessage in outboxMessages
            )
            {
                Exception? error = null;
                try
                {

                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Data, _jsonSerializerSettings);

                    await _publisher.Publish(domainEvent!, context.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox message {Id}", outboxMessage.Id);
                    error = ex;
                }

                await UpdateOutboxMesage(connection, transaction, outboxMessage, error);
            }

            transaction.Commit();

            _logger.LogInformation("Outbox job executed.");
        }

        private async Task UpdateOutboxMesage(IDbConnection connection, IDbTransaction transaction, OutboxMessageData message, Exception? exception)
        {
            var sql = @"

                UPDATE outbox_messages
                SET processed_at = @ProcessedAt,
                    error = @Error
                WHERE id = @Id
            ";

            await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = message.Id,
                    ProcessedAt = _dateTimeProvider.CurrentTime,
                    Error = exception?.Message
                },
                transaction: transaction
            );

        }

    }

    public record OutboxMessageData(Guid Id, string Data);
}