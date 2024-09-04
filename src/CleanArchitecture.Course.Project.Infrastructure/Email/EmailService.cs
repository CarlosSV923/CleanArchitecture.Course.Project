using CleanArchitecture.Course.Project.Application.Abstractions.Email;

namespace CleanArchitecture.Course.Project.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendAsync(Domain.Entities.Users.Email recipient, string subject, string body, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

    }
}