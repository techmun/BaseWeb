using BaseWeb.Cores;
using BaseWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseWeb.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<UserLogin> Users { get; set; }

        private Decrypt decrypt = new Decrypt();

        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor context;
        public AppDbContext( IConfiguration _configuration, IHttpContextAccessor _context)
        {
            configuration = _configuration;
            context = _context;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string constr = decrypt.Decrypted(configuration.GetConnectionString("Default"));
            optionsBuilder.UseMySQL(constr);
        }
    }
}
