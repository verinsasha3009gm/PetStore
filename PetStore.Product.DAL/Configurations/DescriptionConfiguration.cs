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
    public class DescriptionConfiguration : IEntityTypeConfiguration<Description>
    {
        public void Configure(EntityTypeBuilder<Description> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Culture).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Detail).IsRequired().HasMaxLength(200);
        }
    }
}
