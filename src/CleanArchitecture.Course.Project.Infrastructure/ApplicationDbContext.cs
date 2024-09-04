using CleanArchitecture.Course.Project.Application.Exceptions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Course.Project.Infrastructure
{
    public sealed class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IPublisher publisher
    ) : DbContext(options), IUnitOfWork
    {

        private readonly IPublisher _publisher = publisher;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                await PublishDomainEventsAsync();

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConsurrencyException("Excepcion por concurrencia", ex);
            }

        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<IEntity>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    var domainEvents = x.GetDomainEvents();
                    x.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }



    }
}