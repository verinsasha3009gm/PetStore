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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);

            builder.Property(p => p.ProductPassportId).IsRequired();
            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p=>p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasMaxLength(100);

            builder.HasMany(p => p.CartLines)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .HasPrincipalKey(p => p.Id);

            builder.HasIndex(p => p.GuidId);

            builder.HasData(new List<Product>()
            { 
                new Product()
                {
                    Id = 1,
                    GuidId = "00000000-0000-0000-0000-000000000000",
                    Name ="TestName",
                    Description = "TestDescription",
                    Price = 11221,
                    ProductPassportId = 1,
                    CategoryId = 1,
                },
                new Product()
                {
                    Id = 2,
                    GuidId = "00000000-0000-0000-0000-000000000002",
                    Name ="TestName2",
                    Description = "TestDescription",
                    Price = 4655,
                    ProductPassportId = 2,
                    CategoryId = 1,
                },
                new Product()
                {
                    Id = 3,
                    GuidId = "00000000-0000-0000-0000-000000000003",
                    Name ="TestName3",
                    Description = "TestDescription3",
                    Price = 232,
                    CategoryId = 1,
                },
                new Product()
                {
                    Id = 4,
                    GuidId = "00000000-0000-0000-0000-000000000004",
                    Name ="TestName4",
                    Description ="TestDescription",
                    Price= 1223,
                    CategoryId = 1,
                }
            });
        }
    }
}
