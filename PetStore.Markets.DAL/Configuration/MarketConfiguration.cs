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
    public class MarketConfiguration : IEntityTypeConfiguration<Market>
    {
        public void Configure(EntityTypeBuilder<Market> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(p => p.Employes)
                .WithOne(p => p.Market)
                .HasForeignKey(p => p.MarketId)
                .HasPrincipalKey(p => p.Id);

            builder.HasMany(p => p.MarketCapitals)
                .WithOne(p => p.Market)
                .HasForeignKey(p => p.MarketId)
                .HasPrincipalKey(p => p.Id);

            builder.HasOne(p => p.Adress)
               .WithOne(p => p.Market)
               .HasForeignKey<Market>(p => p.AdressId)
               .HasPrincipalKey<Address>(p => p.Id);

            builder.HasMany(p => p.RangeProducts).WithMany(p => p.Markets).UsingEntity<MarketProductLine>(
                p => p.HasOne<ProductLine>().WithMany().HasForeignKey(p => p.ProductLineId),
                p => p.HasOne<Market>().WithMany().HasForeignKey(p => p.MarketId));
            builder.HasData(new List<Market>
            {
                new Market
                {
                    Id = 1,
                    GuidId = "o440ca5f-cb42-40f9-b180-c7bd81c15630",
                    Name ="TestMarket",
                    //AdressId = 3
                },
                new Market
                {
                    Id= 2,
                    Name ="NameMarket",
                    GuidId = "o550ca5f-cb42-40f9-b180-c7bd81c15630",
                    RangeProducts = new List<ProductLine> {},
                    MarketCapitals = new List<MarketCapital>{}
                }
            });
        }
    }
}
