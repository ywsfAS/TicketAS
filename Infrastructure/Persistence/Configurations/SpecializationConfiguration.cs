using Core.Agents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization> 
    {

        public void Configure(EntityTypeBuilder<Specialization> builder)
        {

            builder.ToTable("Specializations");

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)")
                .IsRequired();


            builder.HasIndex(s => s.Name , "UQ_Specializations_Name")
                .IsUnique();
        }
    }
}
