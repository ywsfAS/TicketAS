using Core.Reporters;
using Core.Reporters.ReportStates;
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
                .HasForeignKey<Reporter>(r => r.UserId);

            builder.Property(r => r.State)
                .HasConversion(
                    s => s.Name,
                    value => CreateReporterState(value)
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

        }
        private static ReporterState CreateReporterState(string value)
        {
            return value switch
            {
                "Active" => new ActiveReporterState(),
                "Suspended" => new SuspendedReporterState(),
                "Blocked" => new BlockedReporterState(),
                _ => throw new InvalidOperationException(
                    $"Unknown reporter state: {value}")
            };
        }
    }
}
