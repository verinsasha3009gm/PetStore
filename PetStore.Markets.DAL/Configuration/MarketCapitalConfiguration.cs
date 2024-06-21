using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Markets.Domain.Entity;

namespace PetStore.Markets.DAL.Configuration
{
    public class MarketCapitalConfiguration : IEntityTypeConfiguration<MarketCapital>
    {
        public void Configure(EntityTypeBuilder<MarketCapital> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.DailyIncome).IsRequired().HasMaxLength(100);
            builder.HasData(new List<MarketCapital>
            {
                new MarketCapital()
                {
                    Id = 1,
                    Date = DateTime.UtcNow.AddHours(-21) /*new DateTime(2024,6,5)*/ /*DateTime.ParseExact("05.06.2024 21:24:25", "dd.MM.yyyy HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture)*/,
                    ProductsSold = new List<ProductLine>() {},
                    DailyIncome = 0,
                    MarketId = 2
                }
            });
        }
    }
}
