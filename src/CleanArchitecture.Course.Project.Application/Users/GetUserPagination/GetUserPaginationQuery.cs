using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;
using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserPagination
{
    public record GetUserPaginationQuery :
    PaginationParams,
    IQuery<PagedResult<User, UserId>>
    {

    }
}
