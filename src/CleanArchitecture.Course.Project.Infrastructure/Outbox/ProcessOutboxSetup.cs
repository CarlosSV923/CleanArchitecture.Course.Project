using Microsoft.Extensions.Options;
using Quartz;

namespace CleanArchitecture.Course.Project.Infrastructure.Outbox
{
    public class ProcessOutboxSetup(
        IOptions<OutboxOptions> outboxOptions
    ) : IConfigureOptions<QuartzOptions>
    {

        private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

        public void Configure(QuartzOptions options)
        {
            const string jobName = nameof(InvokeOutboxJob);
            options.AddJob<InvokeOutboxJob>(
                job => job
                    .WithIdentity(jobName)
                    .WithDescription("Invoke Outbox Job")
            );

            options.AddTrigger(
                trigger => trigger
                    .WithIdentity("InvokeOutboxJob-trigger")
                    .ForJob(jobName)
                    .WithSimpleSchedule(
                        schedule => schedule
                            .WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                            .RepeatForever()
                    )
            );
        }
    }
}
