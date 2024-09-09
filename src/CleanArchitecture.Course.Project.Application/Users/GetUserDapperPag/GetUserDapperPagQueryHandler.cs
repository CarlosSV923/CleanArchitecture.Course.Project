using System.Text;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using Dapper;

namespace CleanArchitecture.Course.Project.Application.Users.GetUserDapperPag
{
    internal sealed class GetUserDapperPagQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory
    ) : IQueryHandler<GetUserDapperPagQuery, PagedDapperResult<UserPagData>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
        public async Task<Result<PagedDapperResult<UserPagData>>> Handle(GetUserDapperPagQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var sqlBuilder = new StringBuilder(
                """
                    SELECT
                        usr.email AS Email,
                        rol.name AS Rol,
                        perm.name AS Permiso
                    FROM users usr
                    LEFT JOIN users_roles usr_rol 
                        ON usr.id = usr_rol.user_id   
                    LEFT JOIN roles rol 
                        ON usr_rol.role_id = rol.id
                    LEFT JOIN roles_permissions rol_perm 
                        ON rol.id = rol_perm.role_id
                    LEFT JOIN permissions perm 
                        ON rol_perm.permission_id = perm.id
                """
            );

            var search = string.Empty;
            var whereStatement = string.Empty;

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                search = "%" + EncodeForLike(request.Search) + "%";
                whereStatement = "WHERE usr.email LIKE @Search OR rol.name LIKE @Search OR perm.name LIKE @Search";
                sqlBuilder.AppendLine(whereStatement);
            }

            if(!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                var orderStatement = string.Empty;
                var orderAscDesc = request.IsAscending ? "ASC" : "DESC";

                switch(request.OrderBy)
                {
                    case "Email":
                        orderStatement = $"ORDER BY usr.email {orderAscDesc}";
                        break;
                    case "Rol":
                        orderStatement = $"ORDER BY rol.name {orderAscDesc}";
                        break;
                    case "Permiso":
                        orderStatement = $"ORDER BY perm.name {orderAscDesc}";
                        break;
                }

                sqlBuilder.AppendLine(orderStatement);
            }

            sqlBuilder.AppendLine("LIMIT @PageSize OFFSET @Offset;");

            sqlBuilder.AppendLine(
                """
                SELECT
                    COUNT(*)
                    FROM users usr
                    LEFT JOIN users_roles usr_rol 
                        ON usr.id = usr_rol.user_id   
                    LEFT JOIN roles rol 
                        ON usr_rol.role_id = rol.id
                    LEFT JOIN roles_permissions rol_perm 
                        ON rol.id = rol_perm.role_id
                    LEFT JOIN permissions perm 
                        ON rol_perm.permission_id = perm.id
                """
            );

            if(!string.IsNullOrWhiteSpace(whereStatement))
            {
                sqlBuilder.AppendLine(whereStatement);
            }
            sqlBuilder.AppendLine(";");

            using var multiQuery = await connection.QueryMultipleAsync(sqlBuilder.ToString(), new
            {
                request.PageSize,
                Offset = (request.PageIndex - 1) * request.PageSize,
                Search = search
            });

            var items = await multiQuery.ReadAsync<UserPagData>().ConfigureAwait(false);

            var totalCount = await multiQuery.ReadFirstAsync<int>().ConfigureAwait(false);

            return new PagedDapperResult<UserPagData>(
                totalCount,
                request.PageIndex,
                request.PageSize
            )
            {
                Items = items
            };  

        }   

        private static string EncodeForLike(string value)
        {
            return value
                .Replace("!", "!!")
                .Replace("%", "!%")
                .Replace("_", "!_")
                .Replace("[", "![")
                .Replace("[", "[]]")
                .Replace("%", "[%]");
        }
    }
}