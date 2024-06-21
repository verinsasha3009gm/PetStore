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
    public class MarketCapitalProductLineConfiguration : IEntityTypeConfiguration<MarketCapitalProductLine>
    {
        public void Configure(EntityTypeBuilder<MarketCapitalProductLine> builder)
        {
        }
    }
}
