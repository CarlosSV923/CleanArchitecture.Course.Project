using CleanArchitecture.Course.Project.Application.Paginations;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Course.Project.Infrastructure.Repositories
{
    internal sealed class UserRepository(ApplicationDbContext context) : 
        Repository<User, UserId>(context), 
        IUserRepository,
        IPaginationUserRepository
    {
        public Task<User?> GetByEmailAsync(Domain.Entities.Users.Email email, CancellationToken cancellationToken = default)
        {
            return _context.Set<User>()
                .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
        }

        public Task<bool> IsUserExistsAsync(Domain.Entities.Users.Email email, CancellationToken cancellationToken = default)
        {
            return _context.Set<User>()
                .AnyAsync(user => user.Email == email, cancellationToken);
        }

        public override void Add(User user)
        {
            foreach (var role in user.Roles!)
            {
                _context.Attach(role);
            }

            _context.Add(user);
        }
    }
}