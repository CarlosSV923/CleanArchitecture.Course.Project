using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;

namespace CleanArchitecture.Course.Project.Domain.Entities.Users
{
    public static class UserErros
    {
        public static Error NotFound => new("User.Found", "No existe el usuario solicitado");

        public static Error InvalidCredentials => new("User.InvalidCredentials", "Credenciales inválidas");

        public static Error EmailAlreadyExists => new("User.EmailAlreadyExists", "El email ya está en uso");
        
    }
}