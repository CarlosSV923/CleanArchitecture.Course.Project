using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserDapperPag
{
    public record GetUserDapperPagQuery : 
        PaginationParams, 
        IQuery<PagedDapperResult<UserPagData>>;
}