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

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(id => id.Id, value => new IncidentSpecializationId(value))
                .ValueGeneratedNever();

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

            builder.HasOne<Specialization>()
                .WithMany()
                .HasForeignKey(x => x.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CategoryId, x.SpecializationId }, "UQ_IncidentCategorySpecializations_Category_Specialization")
                .IsUnique();
        }
    }
}
