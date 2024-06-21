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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Street).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.City).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Region).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Country).IsRequired().HasMaxLength(100);
            builder.Property(p => p.MarketId);

            builder.HasData(new List<Address>
            {
                new Address()
                {
                    Id = 1,
                    GuidId = "b119ca5f-cb42-40f9-b180-c7bd81c15651",
                    City ="Moskau",
                    Country = "Russia",
                    Region ="Moskau",
                    Street = "Leninskau 19a"
                },
                new Address()
                {
                    Id = 2,
                    GuidId = "c330ca5f-cb42-40f9-b180-c7bd81c15651",
                    City ="Berlin",
                    Country = "FRG",
                    Region ="Berlin",
                    Street = "Munhenskai 19a"
                },
                new Address()
                {
                    Id = 3,
                    GuidId = "b449ca5f-cb42-40f9-b180-c7bd81c15651",
                    City ="Tula",
                    Country = "Russia",
                    Region ="Tula",
                    Street = "Leninskau 19a",
                    //MarketId = 1
                },
                new Address()
                {
                    Id = 4,
                    GuidId = "q930ca5f-cb42-40f9-b180-c7bd81c15651",
                    City ="Berlin",
                    Country = "GDR",
                    Region ="Berlin",
                    Street = "Munhenskai 19b"
                },
            });
        }
    }
}
