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
    public class EmployeConfiguration : IEntityTypeConfiguration<Employe>
    {
        public void Configure(EntityTypeBuilder<Employe> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Gender).IsRequired().HasMaxLength(100);

            builder.HasOne(p => p.Passport)
               .WithOne(p => p.Employe)
               .HasForeignKey<Employe>(p => p.PassportId)
               .HasPrincipalKey<Passport>(p => p.Id);

            builder.HasOne(p => p.EmployePassport)
               .WithOne(p => p.Employe)
               .HasForeignKey<Employe>(p => p.EmployePassportId)
               .HasPrincipalKey<EmployePassport>(p => p.Id);

            builder.HasOne(p => p.Token)
               .WithOne(p => p.Employe)
               .HasForeignKey<Employe>(p => p.TokenId)
               .HasPrincipalKey<Token>(p => p.Id);
            builder.HasData(new List<Employe>
            {
                new Employe()
                {
                    Id = 1, 
                    Email = "qwertyuiop@gmail.com",
                    Gender ="F",
                    GuidId =Guid.NewGuid().ToString(),
                    Name = "Name",
                    Password ="9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                    EmployePassportId = 1,
                },
                new Employe()
                {
                    Id = 2,
                    Email = "qwertyuiop2@gmail.com",
                    Gender ="M",
                    GuidId ="d111ca5f-cb42-40f9-b180-c7bd81c15651",
                    Name = "NameTestEmploye",
                    Password ="9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                    EmployePassportId = 2,
                },
                new Employe()
                {
                    Id = 3,
                    Email = "qwertyuiop3@gmail.com",
                    Gender ="M",
                    GuidId ="d222ca5f-cb42-40f9-b180-c7bd81c15651",
                    Name = "NameTestEmploye3",
                    Password ="9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                },
                new Employe()
                {
                    Id = 4,
                    Email = "qwertyuiop4@gmail.com",
                    Gender ="M",
                    GuidId ="d333ca5f-cb42-40f9-b180-c7bd81c15651",
                    Name = "NameTestEmploye4",
                    Password ="9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                }
            });
        }
    }
}
