using Core.Agents;
using Core.Agents.Seniority;
using Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {

            builder.ToTable("Agents");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasConversion(id => id.Id, value => new AgentId(value))
                .ValueGeneratedNever();

            builder.Property(a => a.UserId)
                .HasConversion(id => id.Id, value => new UserId(value))
                .IsRequired();

            builder.HasOne(a => a.User)          
                .WithOne()
                .HasForeignKey<Agent>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.Seniority)
                .HasConversion(s => s.Name, value => MapSeniority(value))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.State)
                .HasConversion(s => s.Name, value => DomainLookups.AgentState(value))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.CreatedAt).IsRequired().HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(a => a.UpdatedAt).HasColumnType("datetime2");

            builder.HasMany(a => a.Specializations)
                .WithOne()
                .HasForeignKey(s => s.AgentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(a => a.Specializations)
                .HasField("_agentSpecializations")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(a => a.Incidents)
                .WithOne()
                .HasForeignKey("AssignedAgentId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Navigation(a => a.Incidents)
                .HasField("_incidents")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }

        private static AgentSeniority MapSeniority(string name) =>
            AgentSeniority.FindByName(name) ?? throw new InvalidOperationException($"Unknown seniority '{name}' in database.");
    }
    
}
