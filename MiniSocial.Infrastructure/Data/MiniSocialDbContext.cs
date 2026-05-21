using Microsoft.EntityFrameworkCore;

namespace MiniSocial.Infrastructure.Data;

public class MiniSocialDbContext : DbContext
{
    public MiniSocialDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}