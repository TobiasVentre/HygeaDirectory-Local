using DirectoryMS.Application.Interfaces;
using DirectoryMS.Domain.Entities;
using DirectoryMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DirectoryMS.Infrastructure.Repositories;

public class UserRepository(DirectoryDbContext dbContext) : IUserRepository
{
    public async Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .OrderBy(x => x.CreatedAtUtc)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
