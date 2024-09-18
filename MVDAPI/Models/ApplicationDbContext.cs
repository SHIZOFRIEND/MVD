using Microsoft.EntityFrameworkCore;
namespace MVDAPI.Models
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Sotrudniki> Sotrudniki { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Sotrudniki>()
                    .ToTable("Employee")
                    .HasKey(s => s.IDEmployee);
            }
        }
    }
