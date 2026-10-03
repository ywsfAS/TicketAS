using Core.Agents;
using Core.Incidents.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class IncidentCategorySpecializationConfiguration
        : IEntityTypeConfiguration<IncidentCategorySpecialization>
    {
        public void Configure(
            EntityTypeBuilder<IncidentCategorySpecialization> builder)
        {
            builder.ToTable("IncidentCategorySpecializations");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.CategoryId)
                .HasConversion(
                    id => id.Id,
                    value => new IncidentCategoryId(value))
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(x => x.SpecializationId)
                .HasConversion(
                   id => id.id,
                   value => new SpecializationId(value)
                )
                .HasMaxLength(50)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.HasOne<IncidentCategory>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<IncidentCategorySpecialization>()
                .WithMany()
                .HasForeignKey(x => x.SpecializationId);
        }
    }
}
