using AuthModule.Data.Models;
using AuthModule.Data.Models.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Data;

public class AuthDbContxt<TUser, TUserId> : DbContext where TUser : class, IUser<TUser, TUserId>
{

    public AuthDbContxt(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<TUser> Users { get; set; }

    public DbSet<Claim<TUser>> Claims { get; set; }

    public DbSet<Role<TUser>> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("auth");

        #region TUser Config 
        
        modelBuilder.Entity<TUser>()
            .ToTable("Users")
            .HasKey(x => x.Id);

        modelBuilder.Entity<TUser>()
            .HasIndex(x => x.Handle)
            .IsUnique();

        modelBuilder.Entity<TUser>()
            .Property(x=>x.Handle)
            .IsRequired();
        
        modelBuilder.Entity<TUser>()
            .Property(x=>x.Password)
            .IsRequired();
        
        modelBuilder.Entity<TUser>()
            .HasMany(x => x.Claims)
            .WithMany(x => (IEnumerable<TUser>)x.Users)
            .UsingEntity(j => j.ToTable("UserClaims"));

        modelBuilder.Entity<TUser>()
            .HasMany(x => x.Roles)
            .WithMany(x => (IEnumerable<TUser>)x.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));


        #endregion


        #region Claims Config

        modelBuilder.Entity<Claim<TUser>>()
            .ToTable("Claims")
            .HasKey(x => x.Id);

        modelBuilder.Entity<Claim<TUser>>()
            .Property(x => x.Name)
            .IsRequired();

        #endregion


        #region Roles Config

        modelBuilder.Entity<Role<TUser>>()
            .ToTable("Roles")
            .HasKey(x => x.Id);

        modelBuilder.Entity<Role<TUser>>()
            .HasMany(x => x.Claims)
            .WithMany(x => x.Roles)
            .UsingEntity(j => j.ToTable("RoleClaims"));

        modelBuilder.Entity<Role<TUser>>()
            .Property(x => x.Name)
            .IsRequired();
        
        modelBuilder.Entity<Role<TUser>>()
            .HasIndex(x => x.Name)
            .IsUnique();

        #endregion
    }

}
