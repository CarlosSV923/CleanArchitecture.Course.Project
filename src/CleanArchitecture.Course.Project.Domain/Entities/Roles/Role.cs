using CleanArchitecture.Course.Project.Domain.Entities.Permissions;
using CleanArchitecture.Course.Project.Domain.Entities.Shared;

namespace CleanArchitecture.Course.Project.Domain.Entities.Roles
{
    public sealed class Role(int id, string name) : Enumeration<Role>(id, name)
    {
        public static readonly Role Cliente = new(1, "Cliente");
        public static readonly Role Administrador = new(2, "Administrador");

        public ICollection<Permission>? Permissions { get; set; }
    }
}