using Microsoft.EntityFrameworkCore;

namespace MiniSocial.Infrastructure.Data;

public class MiniSocialDbContext : DbContext
{
    public MiniSocialDbContext(DbContextOptions options) : base(options)
    { }
    
    public DbSet<Domain.Entities.User> Users { get; set; }
    public DbSet<Domain.Entities.Reaction>  Reactions { get; set; }
    public DbSet<Domain.Entities.Post> Posts { get; set; }
    public DbSet<Domain.Entities.Comment> Comments { get; set; }
    public DbSet<Domain.Entities.Follow> Follows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Users.
        base.OnModelCreating(builder);
    }
}