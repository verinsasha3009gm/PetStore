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
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RefreshToken).IsRequired();
            builder.Property(x => x.RefreshTokenExpiryTime).IsRequired();

            builder.HasData(new List<Token>()
            {
                new Token() { Id = 1,
                    RefreshToken = "kXYUB5TkvhOGEJfr3dDcLSV99o/bXYoqXGTSeFyX6Lk=",
                    RefreshTokenExpiryTime = DateTime.UtcNow,
                    UserId = 1,
                },
                new Token() { Id = 2,
                    RefreshToken = "kXYUB5TkvhOGEJfr3dDcLSV99o/bXYoqXGTSeFyX7Lk=",
                    RefreshTokenExpiryTime = DateTime.UtcNow,
                    UserId = 2,
                }
            });
        }
    }
}
