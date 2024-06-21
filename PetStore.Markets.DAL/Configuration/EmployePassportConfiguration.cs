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
    public class EmployePassportConfiguration : IEntityTypeConfiguration<EmployePassport>
    {
        public void Configure(EntityTypeBuilder<EmployePassport> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Salary).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Post).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Experience).IsRequired().HasMaxLength(100);

            builder.HasOne(p => p.Address)
               .WithOne(p => p.EmployePassport)
               .HasForeignKey<EmployePassport>(p => p.AddressId)
               .HasPrincipalKey<Address>(p => p.Id);

            builder.HasData(new List<EmployePassport>
            {
                new EmployePassport()
                {
                    Id = 1,
                    GuidId= Guid.NewGuid().ToString(),
                    Salary = 10000,
                    Post = "Casier",
                    Experience =3.6m,
                    EnployeId = 1,
                },
                new EmployePassport()
                {
                    Id = 2,
                    GuidId= "f111ca5f-cb42-40f9-b180-c7bd81c15651",
                    Salary = 20000,
                    Post = "Casier",
                    Experience =2.6m,
                    EnployeId = 2,
                },
                new EmployePassport()
                {
                    Id = 3,
                    GuidId= "f333ca5f-cb43-40f9-b180-c7bd81c15651",
                    Salary = 20000,
                    Post = "Casier",
                    Experience =2.3m
                }
            });
        }
    }
}
