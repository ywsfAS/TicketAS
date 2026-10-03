using Core;
using Core.Agents;
using Core.Incidents;
using Core.Reporters;
using Core.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {

            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .HasConversion(id => id.Id, value => new TicketId(value))
                .ValueGeneratedNever();

            builder.Property(t => t.Title)
                .HasConversion(t => t.Name, value => TicketTitle.Create(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasConversion(d => d.Description, value => TicketDescription.Create(value))
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(t => t.ReporterId)
                .HasConversion(id => id.Id, value => new ReporterId(value))
                .IsRequired();
            builder.Property(t => t.AgentId)
                .HasConversion(id => id.Id, value => new AgentId(value))
                .IsRequired();
            builder.Property(t => t.IncidentId)
                .HasConversion(id => id.Id, value => new IncidentId(value))
                .IsRequired();

            builder.HasOne(t => t.Reporter).WithMany().HasForeignKey(t => t.ReporterId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Agent).WithMany().HasForeignKey(t => t.AgentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Incident).WithMany().HasForeignKey(t => t.IncidentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Conversation)
                .WithOne()
                .HasForeignKey<Ticket>("ConversationId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Priority)
                .HasConversion(p => p.Level, value => DomainLookups.Priority(value))
                .HasColumnType("int")
                .IsRequired();

            builder.Property(t => t.Lifecycle)
                .HasColumnName("Status")
                .HasConversion(l => l.Name, value => DomainLookups.Lifecycle(value))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.CreatedAt).IsRequired().HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(t => t.UpdatedAt).HasColumnType("datetime2");

            builder.HasIndex(t => t.Lifecycle, "IX_Tickets_Status");
        }
    }
}
