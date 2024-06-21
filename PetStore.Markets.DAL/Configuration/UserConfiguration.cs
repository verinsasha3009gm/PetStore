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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p=>p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.GuidId).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Role).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Email).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Login).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Password).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.CartId).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.TokenId).IsRequired().HasMaxLength(100);
            builder.HasMany(p => p.Adresses)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.Id);

            builder.HasData(new List<User>
            {
                new User
                {
                    Id = 1,
                    Role = "User",
                    Email = "UserEmail@gmail.com",
                    GuidId = "e008ca5f-cb42-40f9-b180-c7bd81c15651",
                    Login ="TestLogin",
                    Password = "9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                    CartId = 11,
                    TokenId = 23
                },
                new User
                {
                    Id = 2,
                    Role = "UserTest",
                    Email = "UserTest2Email@gmail.com",
                    GuidId = Guid.NewGuid().ToString(),
                    Login ="TestLogin#2",
                    Password = "9a-90-04-03-ac-31-3b-a2-7a-1b-c8-1f-09-32-65-2b-80-20-da-c9-2c-23-4d-98-fa-0b-06-bf-00-40-ec-fd",
                    CartId = 12,
                    TokenId = 24
                }
            });
        }
    }
}
