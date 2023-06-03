using ELibrary_UserService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_UserService.Infrastructure.EF.Config;

public class ReactionTypeConfig : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.Property<bool>("_like")
         .UsePropertyAccessMode(PropertyAccessMode.Field)
         .HasColumnName("Like");
    }
}