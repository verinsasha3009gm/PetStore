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
    public class ProductLineConfiguration : IEntityTypeConfiguration<ProductLine>
    {
        public void Configure(EntityTypeBuilder<ProductLine> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Count).IsRequired().HasMaxLength(1000);

            builder.HasMany(p => p.MarketCapitals).WithMany(p => p.ProductsSold).UsingEntity<MarketCapitalProductLine>(
                p => p.HasOne<MarketCapital>().WithMany().HasForeignKey(p => p.MarketCapitalId),
                p => p.HasOne<ProductLine>().WithMany().HasForeignKey(p => p.ProductLineId));
            builder.HasData(new List<ProductLine>()
            {
                new ProductLine
                {
                     Id = 1,
                     GuidId = "q222ca5f-cb42-40q9-b180-c7bd81c15651",
                     Count = 20,
                     ProductId = 1,
                     Markets = new List<Market> {},
                     MarketCapitals = new List<MarketCapital> {}
                }
            });
        }
    }
}
