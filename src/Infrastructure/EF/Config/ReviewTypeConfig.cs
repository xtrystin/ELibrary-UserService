using ELibrary_UserService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_UserService.Infrastructure.EF.Config;
public class ReviewTypeConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
       builder.Property<string>("_content")
        .UsePropertyAccessMode(PropertyAccessMode.Field)
        .HasColumnName("Content");
    }
}
