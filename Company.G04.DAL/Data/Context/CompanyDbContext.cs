using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.G04.DAL.Moudel;
using Microsoft.EntityFrameworkCore;

namespace Company.G04.DAL.Data.Context
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext() : base() 
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = .; Database = CompanyG04; Trusted_Connection = True; TrustServerCertificate=True");

        }
        public DbSet<Department> Departments { get; set; }
    }
}
