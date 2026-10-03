using Core.Reporters;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ReporterConfiguration : IEntityTypeConfiguration<Reporter>
    {

        public void Configure(EntityTypeBuilder<Reporter> builder)
        {
            builder.ToTable("Reporters");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasConversion(ri => ri.Id, value => new ReporterId(value))
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(r => r.UserId)
                .HasConversion(
                    id => id.Id,
                    value => new UserId(value))
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.HasOne(r => r.User)
                .WithOne()
                .HasForeignKey<Reporter>(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.State)
                .HasConversion(
                    s => s.Name,
                    value => DomainLookups.ReporterState(value)
                )
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)")
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime2");

            builder.Property(u => u.UpdatedAt)
                .HasColumnType("datetime2");

            builder.HasIndex(r => r.UserId, "XI_Reporters_UserId");

            builder.HasMany(r => r.Incidents)
                .WithOne(i => i.Reporter)
                .HasForeignKey(i => i.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Navigation(r => r.Incidents)
                .HasField("_Incidents")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
