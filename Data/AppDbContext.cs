using FitnessTakip.Model;
using Microsoft.EntityFrameworkCore;

namespace FitnessTakip.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProgram> UserPrograms { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
    }
}
