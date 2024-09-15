using CleanArchitecture.Course.Project.Application.Abstractions.Clock;
using CleanArchitecture.Course.Project.Application.Exceptions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace CleanArchitecture.Course.Project.Infrastructure
{
    public sealed class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeProvider dateTimeProvider
    // IPublisher publisher
    ) : DbContext(options), IUnitOfWork
    {
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

        // private readonly IPublisher _publisher = publisher;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddDomainEventsToOutboxMsg();

                var result = await base.SaveChangesAsync(cancellationToken);

                // Se comenta implementacio original de publicacion de eventos
                // await PublishDomainEventsAsync();

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConsurrencyException("Excepcion por concurrencia", ex);
            }

        }

        // private async Task PublishDomainEventsAsync()
        // {
        //     var domainEvents = ChangeTracker
        //         .Entries<IEntity>()
        //         .Select(x => x.Entity)
        //         .SelectMany(x =>
        //         {
        //             var domainEvents = x.GetDomainEvents();
        //             x.ClearDomainEvents();
        //             return domainEvents;
        //         })

        //         .ToList();

        //     foreach (var domainEvent in domainEvents)
        //     {
        //         await _publisher.Publish(domainEvent);
        //     }
        // }

        private void AddDomainEventsToOutboxMsg()
        {
            var outboxMsg = ChangeTracker
                .Entries<IEntity>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    var domainEvents = x.GetDomainEvents();
                    x.ClearDomainEvents();
                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(),
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }),
                    _dateTimeProvider.CurrentTime
                ))
                .ToList();

            AddRange(outboxMsg);

        }



    }
}