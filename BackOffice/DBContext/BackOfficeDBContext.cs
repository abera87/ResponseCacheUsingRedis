using BackOffice.Model;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.DBContext
{
    public class BackOfficeDBContext : DbContext
    {
        public BackOfficeDBContext(DbContextOptions<BackOfficeDBContext> options) : base(options) { }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Department> Department { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                        .Property(p => p.DepartmentId)
                        .HasColumnName("DeptId");
            
            modelBuilder.Entity<Department>()
                        .Property(p => p.DepartmentId)
                        .HasColumnName("Id");

            modelBuilder.Entity<Employee>()
                        .HasOne(e => e.Department)
                        .WithMany(d => d.Employees);
        }
    }
}