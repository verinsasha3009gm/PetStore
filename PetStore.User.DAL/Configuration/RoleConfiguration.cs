using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Users.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.DAL.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired();

            builder.HasData(new List<Role>() 
            { 
                new Role()
                {
                    Id = 1,
                    Name ="User"
                },
                new Role()
                {
                    Id = 2,
                    Name = "Admin"
                },
                new Role()
                {
                    Id =3,
                    Name ="TestRole"
                }
            });
        }
    }
}
