using Core.Reporters;
using Core.Tickets.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class MessageConfiguration
    : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Id,
                    value => new MessageId(value))
                .ValueGeneratedNever();

            builder.Property(m => m.ParticipantId)
                .HasConversion(
                    id => id.Id,
                    value => new ReporterId(value))
                .IsRequired();

            builder.ComplexProperty(m => m.Content, content =>
            {
                content.Property(x => x.Title)
                    .HasConversion(
                       t => t.Title,
                       value => MessageTitle.Create(value) 
                     )
                    .HasMaxLength(150)
                    .IsRequired();

                content.Property(x => x.Body)
                    .HasConversion(
                       t => t.Body,
                       value => MessageBody.Create(value) 
                     )
                    .HasMaxLength(5000)
                    .IsRequired();
            });

            builder.Property(m => m.SentAt)
                .IsRequired();
        }
    }
}
