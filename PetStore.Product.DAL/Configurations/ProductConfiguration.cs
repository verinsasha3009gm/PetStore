using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Products.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PetStore.Products.DAL.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
            builder.Property(p => p.Price).IsRequired().HasMaxLength(100);
            builder.HasMany<Description>(p => p.DescriptionList)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .HasPrincipalKey(p => p.Id);
            
            builder.HasMany(p => p.Tegs).WithMany(p => p.Products).UsingEntity<ProductTeg>(
                p => p.HasOne<Teg>().WithMany().HasForeignKey(p => p.TegId),
                p => p.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId));

            builder.HasOne(p => p.ProductPassport)
                .WithOne(p => p.Product)
                .HasForeignKey<ProductPassport>(p => p.ProductId)
                .HasPrincipalKey<Product>(p => p.Id);
            builder.HasData(new List<Product>
            {
                new Product { Id = 1,
                    Name = "Name",
                    Description = "Description",
                    GuidId = Guid.NewGuid().ToString(),
                    Price= 1999,
                    DescriptionList = new List<Description> {},
                    Tegs = new List<Teg> {},
                       
                },
                new Product { Id = 2,
                    Name = "NameTest",
                    Description = "DescriptionTest",
                    GuidId = Guid.NewGuid().ToString(),
                    Price= 2999,
                    DescriptionList = new List<Description> {},
                    Tegs = new List<Teg> {},

                }
            });
        }
    }
}
