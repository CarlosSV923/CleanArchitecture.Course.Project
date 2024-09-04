namespace CleanArchitecture.Course.Project.Domain.Entities.Users
{
    public sealed class UserRole
    {
        public int RoleId { get; set; }
        public UserId? UserId { get; set; }
    }
}