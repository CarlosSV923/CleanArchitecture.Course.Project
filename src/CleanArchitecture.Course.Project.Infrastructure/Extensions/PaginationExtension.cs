using System.Linq.Expressions;
namespace CleanArchitectureq.Course.Project.Infrastructure.Extensions
{
    public static class PaginationExtension {
        public static IQueryable<TEntity> OrderByPropertyOrField<TEntity>(
            this IQueryable<TEntity> source,
            string propertyName,
            bool ascending = true
        )
        {
            var entityType = typeof(TEntity);
            var orderByMethod = ascending ? "OrderBy" : "OrderByDescending";
            var parameterExpression = Expression.Parameter(entityType);
            var propertyExpression = Expression.PropertyOrField(parameterExpression, propertyName);       

            var selector = Expression.Lambda(propertyExpression, parameterExpression);     

            var orderByExpression = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                [entityType, propertyExpression.Type],
                source.Expression,
                selector
            );

            return source.Provider.CreateQuery<TEntity>(orderByExpression);
        }
    }
}