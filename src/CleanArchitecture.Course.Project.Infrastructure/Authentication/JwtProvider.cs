using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Course.Project.Application.Abstractions.Authentication;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Course.Project.Infrastructure.Authentication
{
    public sealed class JwtProvider
    (
        IOptions<JwtOptions> options,
        ISqlConnectionFactory sqlConnectionFactory
    ) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
        private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
        public async Task<string> GenerateToken(User user)
        {

            const string sql = """
                SELECT
                    perm.name,
                FROM users usr
                    LEFT JOIN users_roles usr_rol ON usr.id = usr_rol.user_id
                    LEFT JOIN roles rol ON usr_rol.role_id = rol.id
                    LEFT JOIN roles_permissions rol_perm ON rol.id = rol_perm.role_id
                    LEFT JOIN permissions perm ON rol_perm.permission_id = perm.id   
                WHERE usr.id = @UserId
            """;

            using var connection = _sqlConnectionFactory.CreateConnection();
            var permissions = await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value });

            var permissionCollection = permissions.ToHashSet();


            var claims = new List<Claim>{
                new (JwtRegisteredClaimNames.Sub, user.Id!.Value.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email!.Value),
             };

            foreach(var per in permissionCollection)
            {
                var claim = new Claim(CustomClaims.Permissions, per);
                claims.Add(claim);
            }
            var sigingCredentias = new SigningCredentials(
               new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
               SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                sigingCredentias
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }

}