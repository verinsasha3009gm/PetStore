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
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p=>p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Price).IsRequired().HasMaxLength(20);
            builder.HasMany(p => p.ProductLines)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .HasPrincipalKey(p => p.Id);
            builder.HasData(new List<Product>
            {
                new Product()
                {
                    Id = 1,
                    Name = "Name",
                    GuidId ="o660ca5f-cb42-40f9-b180-c7bd81c15630",
                    Description = "Description",
                    Price = 111,
                         
                }
            });
        }
    }
}
