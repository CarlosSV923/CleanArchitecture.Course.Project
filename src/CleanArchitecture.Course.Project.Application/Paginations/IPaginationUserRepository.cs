using System.Linq.Expressions;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Course.Project.Application.Paginations
{
    public interface IPaginationUserRepository
    {
        Task<PagedResult<User, UserId>> GetPaginationResultAsync(
            Expression<Func<User, bool>>? predicate,
            Func<IQueryable<User>, IIncludableQueryable<User, object>> include,
            int pageIndex,
            int pageSize,
            string orderBy,
            bool isAscending,
            bool disableTracking = true,
            CancellationToken cancellationToken = default
        );
    }
}