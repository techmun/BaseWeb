using BaseWeb.Cores;
using BaseWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseWeb.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<UserLogin> Users { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<JobProcess> JobProcess { get; set; }
        public DbSet<JobHis> JobHis { get; set; }
        public DbSet<DocProcess> DocProcess { get; set; }
        public DbSet<DocHis> DocHis { get; set; }
        public DbSet<CustProf> CustProf { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<ExpiryDate> ExpiryDate { get; set; }
        public DbSet<WebSetup> WebSetup { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string constr = ConnStr.connection();
            optionsBuilder.UseSqlServer(constr);
        }
    }
}
