using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Application.Paginations;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserPagination
{
    internal sealed class GetUserPaginationQueryHandler(
        IPaginationUserRepository paginationRepository
    ) :
        IQueryHandler<GetUserPaginationQuery, PagedResult<User, UserId>>
    {

        private readonly IPaginationUserRepository _paginationRepository = paginationRepository;

        public async Task<Result<PagedResult<User, UserId>>> Handle(
            GetUserPaginationQuery query,
            CancellationToken cancellationToken = default
        )
        {
            var predicate = PredicateBuilder.New<User>(true);
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                predicate = predicate.Or(user => user.Nombre!.Value.Contains(query.Search));
                predicate = predicate.Or(user => user.Email!.Value.Contains(query.Search));

            }

            var result = await _paginationRepository.GetPaginationResultAsync(
                predicate,
                p => p.Include(x => x.Roles!).ThenInclude(y => y.Permissions!),
                query.PageIndex,
                query.PageSize,
                query.OrderBy!,	
                query.IsAscending,
                true,
                cancellationToken
            );

            return result;

        }

    }
}