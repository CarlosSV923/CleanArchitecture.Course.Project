using Asp.Versioning;
using Asp.Versioning.Builder;
using CleanArchitecture.Course.Project.Api.Controllers.Alquileres;
using CleanArchitecture.Course.Project.Api.Documentation;
using CleanArchitecture.Course.Project.Api.Extensions;
using CleanArchitecture.Course.Project.Api.OptionsSetup;
using CleanArchitecture.Course.Project.Application;
using CleanArchitecture.Course.Project.Application.Abstractions.Authentication;
using CleanArchitecture.Course.Project.Infrastructure;
using CleanArchitecture.Course.Project.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
.AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureOptions<ConfigSwaggerOptions>();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.CustomOperationIds(type => type.ToString());
    }
);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello C# .NET!");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var item in descriptions)
            {
                var url = $"/swagger/{item.GroupName}/swagger.json";
                var name = item.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(
                    url,
                    name
                );
            }
        }
    );
}

// app.ApplyMigrations();

app.UseRequestContextLog();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApiVersionSet apiVersionSet = app
                                .NewApiVersionSet()
                                .HasApiVersion(new ApiVersion(1))
                                .Build();

var routGroupBuilder = app.MapGroup("api/v{version:apiVersion}")
                        .WithApiVersionSet(apiVersionSet);

routGroupBuilder.MapAlquilerEndpoints();        


app.Run();
