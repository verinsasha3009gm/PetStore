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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Count).IsRequired();

            builder.HasMany(p => p.Lines)
                .WithOne(p => p.Cart)
                .HasForeignKey(p => p.CartId)
                .HasPrincipalKey(p => p.Id);
            
            //builder.HasOne(p => p.User)
            //   .WithOne(p => p.Cart)
            //   .HasForeignKey<User>(p => p.CartId)
            //   .HasPrincipalKey<Cart>(p => p.Id);
        }
    }
}
