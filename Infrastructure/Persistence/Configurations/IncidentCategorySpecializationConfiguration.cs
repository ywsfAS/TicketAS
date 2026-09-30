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

            builder.HasKey(x => new
            {
                x.CategoryId,
                x.Specialization
            });

            builder.Property(x => x.CategoryId)
                .HasConversion(
                    id => id.Id,
                    value => new IncidentCategoryId(value))
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(x => x.Specialization)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.HasOne<IncidentCategory>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
