using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity, TEntityId>
    (
        ApplicationDbContext context
    )
    where TEntity : Entity<TEntityId>
    where TEntityId : class
    {
        protected readonly ApplicationDbContext _context = context;

        public async Task<TEntity?> GetByIdAsync(
            TEntityId id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }

        public void Add(
            TEntity entity
        )
        {
            _context.Add(entity);
        }

    }
}