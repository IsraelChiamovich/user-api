using Microsoft.EntityFrameworkCore;
using User_api.Models;

namespace User_api.Data
{
    public class AplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options, IConfiguration configuration) : base( options )
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFriends"));
        }

    }
}
