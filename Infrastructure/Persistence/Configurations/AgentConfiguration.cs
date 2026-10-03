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



            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime2");

            builder.Property(a => a.UpdatedAt)
                .HasColumnType("datetime2");

            builder.Property(a => a.Seniority)
                .HasConversion
                (
                   s => s.Name,
                   value => MapSeniority(value)
                )
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)")
                .IsRequired();


            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Agent>(a => a.UserId);

        }

        private static AgentSeniority MapSeniority(string name) => AgentSeniority.FindByName(name);
    }
    
}
