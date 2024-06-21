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
    public class TegConfiguration : IEntityTypeConfiguration<Teg>
    {
        public void Configure(EntityTypeBuilder<Teg> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.HasData(new List<Teg>
            {
                new Teg 
                { 
                    Id = 1,
                    Name ="Fruct"
                },
                new Teg
                {
                    Id =2,
                    Name ="Tomato"
                }
            });
        }
    }
}
