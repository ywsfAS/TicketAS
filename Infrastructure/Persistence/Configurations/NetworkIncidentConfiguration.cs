using Core.Incidents.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class NetworkIncidentConfiguration : IEntityTypeConfiguration<NetworkIncident>
{
    public void Configure(EntityTypeBuilder<NetworkIncident> builder)
    {
        builder.ToTable("NetworkIncidentCategories");

        builder.HasKey(network => network.CategoryId);
        builder.Property(network => network.CategoryId)
            .HasConversion(id => id.Id, value => new IncidentCategoryId(value))
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        builder.HasOne(network => network.Category)
            .WithOne()
            .HasForeignKey<NetworkIncident>(network => network.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(network => network.Symptom)
            .HasConversion<byte>()
            .HasColumnType("tinyint")
            .IsRequired();

        builder.Ignore(network => network.Name);
    }
}
