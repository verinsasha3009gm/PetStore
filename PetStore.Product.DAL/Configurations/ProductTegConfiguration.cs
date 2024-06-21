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
    public class ProductTegConfiguration : IEntityTypeConfiguration<ProductTeg>
    {
        public void Configure(EntityTypeBuilder<ProductTeg> builder)
        {

        }
    }
}
