using Core.Tickets.Conversation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class ConversationConfiguration
    : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                    id => id.Id,
                    value => new ConversationId(value))
                .ValueGeneratedNever();

            builder.HasOne(c => c.Reporter)
                .WithMany()
                .HasForeignKey("ReporterId")
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Ignore(c => c.AgentsIds);
        }
    }
}
