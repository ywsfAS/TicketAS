using Core;
using Core.Agents;
using Core.Incidents;
using Core.Reporters;
using Core.Tickets;
using Core.Tickets.TicketPriotities;
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
                .HasConversion(
                    id => id.Id,
                    value => new TicketId(value)
                 )
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(t => t.Title)
                .HasConversion(
                    ti => ti.Name,
                    value => TicketTitle.Create(value)
                )
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(t => t.Description)
                .HasConversion(
                    d => d.Description,
                    value => TicketDescription.Create(value)
                )
                .HasColumnType("nvarchar(2000)")
                .IsRequired();

            builder.Property(t => t.ReporterId)
                .HasConversion(
                    r => r.Id,
                    value => new ReporterId(value)
                )
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(t => t.AgentId)
                .HasConversion(
                    r => r.Id,
                    value => new AgentId(value)
                )
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(t => t.IncidentId)
                .HasConversion(
                    r => r.Id,
                    value => new IncidentId(value)
                )
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(t => t.Priority)
                .HasConversion(
                   p => p.Level,
                   value => MapPriority(value)
                )
                .HasColumnType("integer")
                .IsRequired();


            builder.HasOne<Reporter>()
                .WithMany()
                .HasForeignKey(x => x.ReporterId);

            builder.HasOne<Agent>()
                .WithMany()
                .HasForeignKey(x => x.AgentId);

            builder.HasOne<Incident>()
                .WithMany()
                .HasForeignKey(x => x.IncidentId);

        }
        public static TicketPriority MapPriority(int level)
        {
            return level switch
            {
                0 => new LowTicket(),
                1 => new NormalTicket(),
                2 => new HighTicket(),
                3 => new CriticalTicket()
            };

        }
    }
}
