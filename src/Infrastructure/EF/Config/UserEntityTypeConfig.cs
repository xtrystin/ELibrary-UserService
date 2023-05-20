using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibrary_UserService.Infrastructure.EF.Config;
internal class UserEntityTypeConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);

        builder.Property<Description>("_description")
           .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnName("Description")
           .HasMaxLength(Description.MaxLength)
           .HasConversion(v => v.Value, v => new Description(v));

        builder.Property<string>("_firstName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("FirstName");

        builder.Property<string>("_lastName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("LastName");

        builder.Property<bool>("_isAccountBlocked")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsAccountBlocked");

        builder.Property<decimal>("_amountToPay")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("AmountToPay");


    }
}
