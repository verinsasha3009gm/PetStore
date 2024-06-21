using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Products.Domain.Entity;
using static System.Net.Mime.MediaTypeNames;

namespace PetStore.Products.DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);

            builder.HasMany(p => p.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .HasPrincipalKey(p => p.Id);

            builder.HasData(new Category()
            {
                Id = 1,
                Name = "name",
                Description = "description",
                Products = new List<Product>() { }
            });
        }
    }
}
