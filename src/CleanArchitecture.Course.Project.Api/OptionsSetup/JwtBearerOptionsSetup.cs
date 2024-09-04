using System.Text;
using CleanArchitecture.Course.Project.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Course.Project.Api.OptionsSetup
{
    public class JwtBearerOptionsSetup
    (
        IOptions<JwtOptions> jwtOptions
    )
     : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        public void Configure(string? name, JwtBearerOptions options)
        {
            ConfigureToken(options);

        }

        public void Configure(JwtBearerOptions options)
        {
            ConfigureToken(options);
        }

        private void ConfigureToken(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!))
            };
        }
    }
}