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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.ApplyMigrations();

app.UseRequestContextLog();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
