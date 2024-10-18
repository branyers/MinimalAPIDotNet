using Microsoft.EntityFrameworkCore;
using MimimalAPiPeliculas.Entities;

namespace MimimalAPiPeliculas;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Gender>().Property(p => p.Name).HasMaxLength(50);
        modelBuilder.Entity<Actor>().Property(p=> p.Name).HasMaxLength(150);
        modelBuilder.Entity<Actor>().Property(p=> p.Picture).IsUnicode();
    }

    public DbSet<Gender> Genders { get; set; }
    public DbSet<Actor> Actors { get; set; }
    
}