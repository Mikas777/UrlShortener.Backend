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
        const string userUserId = "7D5F1E2C-8F4C-4C3A-9D6A-2E8F4B5C9A1B";

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
                },
                new UserDAO
                {
                    Id = Guid.Parse(userUserId),
                    Username = "user",
                    NormalizedUsername = "USER",
                    PasswordHash = "user"
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
                        },
                        new Dictionary<string, object>
                        {
                            { "RolesId", Guid.Parse(userRoleId) },
                            { "UsersId", Guid.Parse(userUserId) }
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

            var seedDate = DateTime.Parse("2025-10-19 23:50:31.486444+03").ToUniversalTime();

            entity.HasData(new UrlDAO
            {
                Id = Guid.Parse("814298ac-bc7e-41d4-98b7-0e21db0ae10a"),
                OriginalUrl = "https://localhost:7111/Login",
                ShortCode = "aDGV9IW",
                CreatedDate = seedDate,
                CreatedById = Guid.Parse(adminUserId)
            }, new UrlDAO
            {
                Id = Guid.Parse("9aa94be8-cbd6-49ba-831f-061131678d81"),
                OriginalUrl = "http://localhost:5173/user-created",
                ShortCode = "RZO1LGP",
                CreatedDate = seedDate,
                CreatedById = Guid.Parse(userUserId)
            }, new UrlDAO
            {
                Id = Guid.Parse("fdbf0efa-3a81-46ad-a1fd-827ed6183ac2"),
                OriginalUrl = "http://localhost:5173/admin-created",
                ShortCode = "37f2Fmo",
                CreatedDate = seedDate,
                CreatedById = Guid.Parse(adminUserId)
            });
        });
        
        base.OnModelCreating(modelBuilder);
    }
}