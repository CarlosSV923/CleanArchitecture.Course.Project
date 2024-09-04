using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Roles;
using CleanArchitecture.Course.Project.Domain.Entities.Users.Events;

namespace CleanArchitecture.Course.Project.Domain.Entities.Users
{
    public sealed class User : Entity<UserId>
    {
        private User() { }
        private User(
            UserId id,
            Name? nombre,
            LastName? apellido,
            Email? email,
            PasswordHash? passwordHash
        ) : base(id)
        {
            PasswordHash = passwordHash;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
        }

        public Name? Nombre { get; private set; }
        public LastName? Apellido { get; private set; }

        public Email? Email { get; private set; }

        public PasswordHash? PasswordHash { get; private set; }

        public static User Create(Name nombre, LastName apellido, Email email, PasswordHash passwordHash)
        {
            var user = new User(UserId.New(), nombre, apellido, email, passwordHash);
            user.AddDomainEvent(new UserCreatedDomainEvent(user.Id!));
            return user;
        }

        public ICollection<Role>? Roles { get; set; }
    }

}