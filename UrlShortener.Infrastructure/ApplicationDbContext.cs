using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Enums;
using UrlShortener.Infrastructure.DAOs;

namespace UrlShortener.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserDAO> Users { get; set; } = null!;
    public DbSet<RoleDAO> Roles { get; set; } = null!;
    public DbSet<UrlDAO> Urls { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        const string adminRoleId = "3BE5C12B-ADB4-49F1-A10D-BDF648BCB40C";
        const string userRoleId = "C10079DB-6A58-41D4-B1FC-B6CE4C7D860F";
        const string adminUserId = "1BF365A0-3E82-4DE8-BEA3-F644A541EBC0";

        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<UserDAO>(entity =>
        {
            entity.ToTable("Users").HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.HasIndex(u => u.NormalizedUsername).IsUnique();

            entity.HasData(
                new UserDAO
                {
                    Id = Guid.Parse(adminUserId),
                    Username = "admin",
                    NormalizedUsername = "ADMIN",
                    PasswordHash = "admin"
                }
            );
        });

        modelBuilder.Entity<RoleDAO>(entity =>
        {
            entity.ToTable("Roles").HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.HasData(
                new RoleDAO
                {
                    Id = Guid.Parse(adminRoleId),
                    Name = nameof(Role.Admin)
                },
                new RoleDAO
                {
                    Id = Guid.Parse(userRoleId),
                    Name = nameof(Role.User)
                }
            );
        });


        modelBuilder.Entity<UserDAO>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRoles",
                j => j.HasOne<RoleDAO>().WithMany().HasForeignKey("RolesId"),
                j => j.HasOne<UserDAO>().WithMany().HasForeignKey("UsersId"),
                j =>
                {
                    j.ToTable("UserRoles");
                    j.HasData(
                        new Dictionary<string, object>
                        {
                            { "RolesId", Guid.Parse(adminRoleId) },
                            { "UsersId", Guid.Parse(adminUserId) }
                        }
                    );
                }
            );

        modelBuilder.Entity<UrlDAO>(entity =>
        {
            entity.ToTable("Urls");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.ShortCode).IsUnique();
            entity.HasIndex(e => e.OriginalUrl);

            entity.HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}