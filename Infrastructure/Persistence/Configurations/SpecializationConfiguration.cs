using Core.Agents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization> 
    {
        public static readonly Guid NetworkId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid InfrastructureId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid DatabaseId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.ToTable("Specializations");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasConversion(id => id.id, value => new SpecializationId(value))   
                .ValueGeneratedNever();

            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);

            builder.HasIndex(s => s.Name, "UQ_Specializations_Name").IsUnique();

            builder.HasData(
                new { Id = new SpecializationId(NetworkId), Name = "Network" },
                new { Id = new SpecializationId(InfrastructureId), Name = "Infrastructure" },
                new { Id = new SpecializationId(DatabaseId), Name = "Database" });
        }

    }
}
