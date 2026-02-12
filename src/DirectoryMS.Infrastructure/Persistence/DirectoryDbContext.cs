using DirectoryMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryMS.Infrastructure.Persistence;

public class DirectoryDbContext(DbContextOptions<DirectoryDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
