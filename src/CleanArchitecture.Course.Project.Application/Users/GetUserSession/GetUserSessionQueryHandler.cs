using CleanArchitecture.Course.Project.Application.Abstractions.Authentication;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Dapper;
using MediatR;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserSession
{
    internal sealed class GetUserSessionQueryHandler : IQueryHandler<GetUserSessionQuery, UserResponse>
    {
        private readonly IUserContext _userContext;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserSessionQueryHandler(IUserContext userContext, ISqlConnectionFactory sqlConnectionFactory)
        {
            _userContext = userContext;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        async Task<Result<UserResponse>> IRequestHandler<GetUserSessionQuery, Result<UserResponse>>.Handle(GetUserSessionQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = @"
                SELECT
                    id as Id,
                    email as Email,
                    name as Name,
                    last_name as LastName
                FROM users
                WHERE email = @id
            ";

            var userResponse = await connection.QuerySingleAsync<UserResponse>(sql, new { id = _userContext.GetUserId });

            return userResponse;
        }
    }   
}