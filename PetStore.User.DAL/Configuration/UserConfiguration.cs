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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Login).IsRequired().HasMaxLength(100);

            builder.HasMany(p => p.Addresses)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.Id);

            builder.HasOne(p => p.Cart)
               .WithOne(p => p.User)
               .HasForeignKey<Cart>(p => p.UserId)
               .HasPrincipalKey<User>(p => p.Id);

            builder.HasOne(p => p.Token)
               .WithOne(p => p.User)
               .HasForeignKey<Token>(p => p.UserId)
               .HasPrincipalKey<User>(p => p.Id);

            builder.HasMany(p => p.Roles).WithMany(p => p.Users).UsingEntity<UserRole>(
                p => p.HasOne<Role>().WithMany().HasForeignKey(p => p.RoleId),
                p => p.HasOne<User>().WithMany().HasForeignKey(p => p.UserId));
            builder.HasData(new List<User>
            {
                //password qwertyuiop
                new User { 
                    Id = 1, Email ="TestEmail@gmail.com"
                    , Login = "TestLogin", GuidId = Guid.NewGuid().ToString()
                    , Password ="9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd"
                    ,TokenId = 1,Addresses = new List<Address>{ } },
                new User
                {
                    Id = 2, Email="TestEmail2@gmail.com"
                    , Login ="TestLogin2", GuidId =Guid.NewGuid().ToString(),
                    Password = "9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                    TokenId = 2
                }
            });
        }
    }
}
