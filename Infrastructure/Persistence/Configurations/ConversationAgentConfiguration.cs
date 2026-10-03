using Core.Agents;
using Core.Tickets.Conversation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class ConversationAgentConfiguration
    : IEntityTypeConfiguration<ConversationAgent>
    {
        public void Configure(EntityTypeBuilder<ConversationAgent> builder)
        {
            builder.ToTable("ConversationAgents");

            builder.HasKey(x => new
            {
                x.ConversationId,
                x.AgentId
            });

            builder.Property(x => x.ConversationId)
                .HasConversion(
                    id => id.Id,
                    value => new ConversationId(value))
                .ValueGeneratedNever();

            builder.Property(x => x.AgentId)
                .HasConversion(
                    id => id.Id,
                    value => new AgentId(value))
                .ValueGeneratedNever();

            builder.HasOne<Conversation>()
                .WithMany()
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Agent>()
                .WithMany()
                .HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
