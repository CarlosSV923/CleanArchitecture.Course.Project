namespace CleanArchitecture.Course.Project.Application.Users.RegisterUser
{
    public record RegisterUserRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

    }
}
