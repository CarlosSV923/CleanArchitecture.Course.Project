using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Permissions
{
    public sealed class Permission : Entity<PermissionId>
    {
        public Name? Name { get; private set; }

        public Permission(PermissionId id, Name name) : base(id)
        {
            Name = name;
        }

        public Permission()
        {

        }

        public Permission(Name name) : base()
        {
            Name = name;
        }

        public static Result<Permission> Create(Name name)
        {
            return new Permission(name);
        }
    }
}