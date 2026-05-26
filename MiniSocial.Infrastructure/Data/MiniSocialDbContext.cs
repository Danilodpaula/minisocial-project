using Microsoft.EntityFrameworkCore;
using MiniSocial.Infrastructure.Data.Models;

namespace MiniSocial.Infrastructure.Data;

public class MiniSocialDbContext : DbContext
{
    public MiniSocialDbContext(DbContextOptions options) : base(options)
    { }
    
    public DbSet<EfcUser> Users { get; set; }
    public DbSet<EfcFollow> Follows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<EfcUser>()
            .HasKey(e => e.Id);
        builder.Entity<EfcFollow>()
            .HasOne<EfcUser>()
            .WithMany()
            .HasForeignKey(f => f.FollowerId);
    }
}