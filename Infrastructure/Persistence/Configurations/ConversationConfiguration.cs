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
                .HasConversion(id => id.Id, value => new ConversationId(value))
                .ValueGeneratedNever();

            builder.HasOne(c => c.Reporter)
                .WithMany()
                .HasForeignKey("ReporterId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.CreatedAt).IsRequired().HasColumnType("datetime2");
            builder.Property(c => c.UpdatedAt).HasColumnType("datetime2");

            builder.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey("ConversationId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Messages)
                .HasField("_messages")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(c => c.Participants)
                .WithOne()
                .HasForeignKey(p => p.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Participants)
                .HasField("_participants")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(c => c.AgentsIds);   
        }
    }
}
