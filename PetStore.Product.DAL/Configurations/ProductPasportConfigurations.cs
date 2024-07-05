using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.DAL.Configurations
{
    public class ProductPasportConfigurations : IEntityTypeConfiguration<ProductPassport>
    {
        public void Configure(EntityTypeBuilder<ProductPassport> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Company).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);

            builder.HasIndex(p => p.GuidId);

            builder.HasData(new List<ProductPassport>
            {
                new ProductPassport()
                {
                     Id = 1,
                     Name = "Name",
                     Company ="NovaCompany",
                     Description ="ctucytc",
                     GuidId = Guid.NewGuid().ToString(),
                },
                new ProductPassport()
                {
                     Id = 2,
                     Name = "NameTest",
                     Company ="NovaCompany2",
                     Description ="ctucytc2",
                     GuidId = Guid.NewGuid().ToString(),
                },
                new ProductPassport()
                {
                     Id = 3,
                     Name = "NameTest3",
                     Company ="NovaCompany3",
                     Description ="ctucytc3",
                     GuidId = Guid.NewGuid().ToString(),
                }
            }); 
        }
    }
}
