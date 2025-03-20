using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G04.DAL.Moudel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.G04.DAL.Data.Configurations
{
    public class EmployeeCofiguration : IEntityTypeConfiguration<Employee>
    {
        void IEntityTypeConfiguration<Employee>.Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");
            builder.HasOne(E => E.Department)
                                .WithMany(D => D.Employees)
                                .HasForeignKey(E => E.DepartmentId)
                                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
