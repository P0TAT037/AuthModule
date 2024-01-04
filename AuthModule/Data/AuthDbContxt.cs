using AuthModule.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Data
{
    public class AuthDbContxt<TUser, TUserId> : DbContext where TUser : class, IUser<TUserId>
    {

        public AuthDbContxt(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TUser> Users { get; set; }

        public DbSet<Claim<TUser>> Claims { get; set; }

        public DbSet<Role<TUser>> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("Auth");

            modelBuilder.Entity<TUser>()
                .ToTable("Users")
                .HasKey(x => x.Id);

            modelBuilder.Entity<TUser>()
                .HasMany(x => x.Claims)
                .WithMany(x => (IEnumerable<TUser>)x.Users)
                .UsingEntity(j => j.ToTable("UserClaims"));

            modelBuilder.Entity<TUser>()
                .HasMany(x => x.Roles)
                .WithMany(x => (IEnumerable<TUser>)x.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            modelBuilder.Entity<Claim<TUser>>()
                .ToTable("Claims")
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role<TUser>>()
                .ToTable("Roles")
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role<TUser>>()
                .HasMany(x => x.Claims)
                .WithMany(x => x.Roles)
                .UsingEntity(j => j.ToTable("RoleClaims"));
        }

    }
}
