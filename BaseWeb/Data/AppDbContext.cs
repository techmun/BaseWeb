using BaseWeb.Cores;
using BaseWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseWeb.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<UserLogin> Users { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<DAProcess> DAProcess { get; set; }
        public DbSet<CustProf> CustProf { get; set; }
        public DbSet<Documents> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string constr = ConnStr.connection();
            optionsBuilder.UseMySQL(constr);
        }
    }
}
