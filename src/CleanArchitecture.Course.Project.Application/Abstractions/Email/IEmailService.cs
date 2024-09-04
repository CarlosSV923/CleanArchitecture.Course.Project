namespace CleanArchitecture.Course.Project.Application.Abstractions.Email
{
    public interface IEmailService
    {   
        Task SendAsync(Domain.Entities.Users.Email recipient, string subject, string body, CancellationToken cancellationToken = default);

    }

}