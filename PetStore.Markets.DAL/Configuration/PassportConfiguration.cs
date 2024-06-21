using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Markets.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.DAL.Configuration
{
    public class PassportConfiguration : IEntityTypeConfiguration<Passport>
    {
        public void Configure(EntityTypeBuilder<Passport> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Issued).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PlaceOfBirth).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.PassportSeria).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.PassportNumber).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.DepartmentCode).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Familien).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PassportDateIssue).IsRequired().HasMaxLength(100);
            builder.Property(p => p.DateOfBirth).IsRequired().HasMaxLength(100);

            builder.HasData(new List<Passport>
            {
                new Passport 
                { 
                    Id = 1,
                    Name = "NamePassport",
                    DateOfBirth = DateTime.UtcNow,
                    Familien ="Familien",
                    Issued = "qqqq",
                    DepartmentCode ="wwww",
                    PassportNumber = 145345,
                    PassportDateIssue = DateTime.UtcNow,
                    PassportSeria = "123121",
                    PlaceOfBirth = "eeee",
                }
            });
        }
    }
}
