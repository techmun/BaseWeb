using Microsoft.EntityFrameworkCore;

namespace BaseWeb.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserLogin> Users { get; set; }

        private readonly IConfiguration configuration;
        public AppDbContext(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string myDb1ConnectionString = this.configuration.GetConnectionString("Default");
            optionsBuilder.UseMySQL(myDb1ConnectionString);
        }
    }
}
