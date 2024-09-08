using System.Linq.Expressions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Infrastructure.Specifications;
using CleanArchitectureq.Course.Project.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Win32.SafeHandles;

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

        public IQueryable<TEntity> ApplaySpecification(
            ISpecification<TEntity, TEntityId> specification
        )
        {
            return SpecificationEvaluator<TEntity, TEntityId>.GetQuery(_context.Set<TEntity>().AsQueryable(), specification);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(
            ISpecification<TEntity, TEntityId> specification,
            CancellationToken cancellationToken = default
        )
        {
            return await ApplaySpecification(specification).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            ISpecification<TEntity, TEntityId> specification,
            CancellationToken cancellationToken = default
        )
        {
            return await ApplaySpecification(specification).CountAsync(cancellationToken);
        }

        public async Task<PagedResult<TEntity, TEntityId>> GetPaginationResultAsync(
            Expression<Func<TEntity, bool>>? predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            int pageIndex,
            int pageSize,
            string orderBy,
            bool isAscending,
            bool disableTracking = true,
            CancellationToken cancellationToken = default
        )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            var skipAmount = pageSize * (pageIndex - 1);

            var totalRecords = await query.CountAsync(cancellationToken);

            List<TEntity>? records;
            if (string.IsNullOrEmpty(orderBy))
            {
                records = await query
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                records = await query
                    .OrderByPropertyOrField(orderBy, isAscending)
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }

            var mod = totalRecords % pageSize;
            var totalPages = (totalRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResult<TEntity, TEntityId>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalRecords,
                TotalPages = totalPages,
                Results = records
            };
        }

    }
}