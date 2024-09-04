using CleanArchitecture.Course.Project.Api.Middleware;
using CleanArchitecture.Course.Project.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {

        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try{

                var context = service.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                

            }catch(Exception ex){
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
            
        } 

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseRequestContextLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestContextLogMiddleware>();
        }
    }
}