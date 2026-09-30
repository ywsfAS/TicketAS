using Core.Agents.Seniority;
using Core.Incidents.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class IncidentCategoryConfiguration : IEntityTypeConfiguration<IncidentCategory>
    {
        public void Configure(EntityTypeBuilder<IncidentCategory> builder)
        {

            builder.ToTable("IncidentCategories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Id,
                    value => new IncidentCategoryId(value))
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(x => x.RequiredSeniority)
                .HasConversion(
                    seniority => seniority.Name,
                    name => AgentSeniority.FindByName(name)
                 )
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)")
                .IsRequired();

            builder.Property(x => x.SpecializationMatchRule)
                .HasConversion<string>()
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)")
                .IsRequired();


        }
    }
}
