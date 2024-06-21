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
    public class AddressConfiguration: IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Street).IsRequired().HasMaxLength(100);
            builder.Property(p => p.City).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Country).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Region).IsRequired().HasMaxLength(100);

            builder.HasData(new List<Address>
            {
                new Address { Id = 1,
                    Street ="Leninskaia 19a", City ="Moskay", Country ="Russia",
                    GuidId = "e009ca5f-cb42-40f9-b180-c7bd81c15652", UserId = 1,
                    Region = "Moskay"
                },
                new Address { Id = 2,
                    Street ="Leninskaia 20a", City ="Moskay", Country ="Russia",
                    GuidId = Guid.NewGuid().ToString(), UserId = 2,
                    Region = "Moskay"
                },
            });
        }
    }
}
