using Core.Agents;
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

            builder.Property(x => x.AgentId)
                .HasConversion(id => id.Id, value => new AgentId(value))
                .IsRequired();

            builder.Property(x => x.SpecializationId)
                .HasConversion(id => id.id, value => new SpecializationId(value))
                .IsRequired();


            builder.HasOne<Specialization>()
                .WithMany()
                .HasForeignKey(x => x.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(x => new { x.AgentId, x.SpecializationId }, "UQ_AgentSpecializations_Agent_Specialization")
                .IsUnique();

        }
    }
}
