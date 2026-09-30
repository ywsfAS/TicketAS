using Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(sid => sid.Id, value => new UserId(value))
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();


            builder.Property(u => u.UserName)
                .HasConversion(userName => userName.Name, value => UserName.Create(value))
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(u => u.Password)
                .HasConversion(pass => pass.Password, value => UserPassword.Create(value))
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.Property(u => u.Email)
                .HasConversion(email => email.Address, value => Email.Create(value))
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(u => u.PhoneNumber)
                .HasConversion(p => p.Phone, value => PhoneNumber.Create(value))
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnType("nvarchar(16)");

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime2");

            builder.Property(u => u.UpdatedAt)
                .HasColumnType("datetime2");

            builder.HasIndex(u => u.UserName,"IX_Users_UserName");
            builder.HasIndex(u => u.Email, "UQ_Users_Email")
                .IsUnique();

        }
    }
}
