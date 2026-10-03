using Core.Incidents;
using Core.Incidents.Categories;
using Core.Reporters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {

        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.ToTable("Incidents");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .HasConversion(id => id.Id, value => new IncidentId(value))
                .ValueGeneratedNever();

            builder.Property(i => i.ReporterId)
                .HasConversion(id => id.Id, value => new ReporterId(value))
                .IsRequired();

            builder.Property(i => i.CategoryId)
                .HasConversion(id => id.Id, value => new IncidentCategoryId(value))
                .IsRequired();

            builder.HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.Title)
                .HasConversion(t => t.Problem + "-" + t.AffectedService, value => IncidentTitle.Create(value))
                .HasMaxLength(350)
                .IsRequired();

            builder.Property(i => i.Description)
                .HasConversion(d => d.Description, value => IncidentDescription.Create(value))
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.Environment)
                .HasConversion(e => e.Name, value => DomainLookups.Environment(value))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.Scope)
                .HasConversion(s => s.Name, value => DomainLookups.Scope(value))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.IncidentSeverity)
                .HasConversion(s => s.Name, value => DomainLookups.Severity(value))
                .HasMaxLength(10)
                .IsRequired();

            builder.OwnsOne(i => i.IncidentSla, sla =>
            {
                sla.OwnsOne(s => s.Acknowledgement, d =>
                {
                    d.Property(x => x.Duration).HasColumnName("AckDuration").HasConversion<long>().IsRequired();
                    d.Property(x => x.DueAt).HasColumnName("AckDueAt").HasColumnType("datetime2").IsRequired();
                });
                sla.OwnsOne(s => s.Resolution, d =>
                {
                    d.Property(x => x.Duration).HasColumnName("ResolutionDuration").HasConversion<long>().IsRequired();
                    d.Property(x => x.DueAt).HasColumnName("ResolutionDueAt").HasColumnType("datetime2").IsRequired();
                });
                sla.Navigation(s => s.Acknowledgement).IsRequired();
                sla.Navigation(s => s.Resolution).IsRequired();
            });
            builder.Navigation(i => i.IncidentSla).IsRequired();
            builder.Ignore(i => i.Behavior);

            builder.Property(i => i.CreatedAt).IsRequired().HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(i => i.UpdatedAt).HasColumnType("datetime2");
        }
    }
}
