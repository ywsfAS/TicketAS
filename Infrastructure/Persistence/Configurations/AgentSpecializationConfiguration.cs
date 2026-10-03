using Core.Agents;
using Core.Incidents.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AgentSpecializationConfiguration : IEntityTypeConfiguration<AgentSpecialization>
    {

        public void Configure(EntityTypeBuilder<AgentSpecialization> builder)
        {

            builder.ToTable("AgentSpecializations");


            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Id,
                    value => new AgentSpecializationId(value)
                )
                .HasMaxLength(50)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.HasOne<Agent>()
                .WithMany()
                .HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AgentSpecialization>()
                .WithMany()
                .HasForeignKey(x => x.SpecializationId);

        }
    }
}
