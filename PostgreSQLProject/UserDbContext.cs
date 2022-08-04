using Microsoft.EntityFrameworkCore;

namespace PostgreSQLProject
{
    class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=root1234;Database=myDB");
        }
    }
}
