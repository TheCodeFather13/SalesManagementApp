using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Entities;

namespace SalesManagementApp.Data
{
    public class SalesManagementDbContext : DbContext
    {
        public SalesManagementDbContext(DbContextOptions<SalesManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData.AddEmployeeData(modelBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeJobTitle> EmployeeJobTitles { get; set; }


    }
}
