using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Infrastructure.Specifications
{
    public class SpecificationEvaluator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
    {

        public static IQueryable<TEntity> GetQuery(
            IQueryable<TEntity> inputQuery,
            ISpecification<TEntity, TEntityId> specification
        )
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }

            query = specification.Includes!
                        .Aggregate(query, (current, include) => current.Include(include))
                        .AsSplitQuery()
                        .AsNoTracking();

            return query;
        }

    }
}