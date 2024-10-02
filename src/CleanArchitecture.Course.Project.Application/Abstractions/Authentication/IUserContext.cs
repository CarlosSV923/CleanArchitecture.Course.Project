namespace CleanArchitecture.Course.Project.Application.Abstractions.Authentication
{
    public interface IUserContext
    {
        string GetEmail { get; }
        Guid GetUserId { get; }
    }
}