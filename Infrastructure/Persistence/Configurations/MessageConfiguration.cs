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
                .HasConversion(id => id.Id, value => new MessageId(value))
                .ValueGeneratedNever();

            builder.Property(m => m.ParticipantId)
                .HasConversion(id => DomainLookups.ParticipantToDb(id), value => DomainLookups.ParticipantFromDb(value))
                .HasMaxLength(38)
                .IsRequired();

            builder.OwnsOne(m => m.Content, content =>
            {
                content.Property(x => x.Title)
                    .HasColumnName("Title")
                    .HasConversion(t => t.Title, value => MessageTitle.Create(value))
                    .HasMaxLength(150)
                    .IsRequired();

                content.Property(x => x.Body)
                    .HasColumnName("Body")
                    .HasConversion(b => b.Body, value => MessageBody.Create(value))
                    .HasMaxLength(5000)
                    .IsRequired();
            });
            builder.Navigation(m => m.Content).IsRequired();

            builder.Property(m => m.SentAt).IsRequired().HasColumnType("datetime2");
        }
    }
}
