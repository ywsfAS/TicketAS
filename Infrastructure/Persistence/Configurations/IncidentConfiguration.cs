using Core.Incidents;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;
using Core.Reporters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {

        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.ToTable("Incidents");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Id,
                    value => new IncidentId(value))
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(i => i.ReporterId)
                .HasConversion(
                    id => id.Id,
                    value => new ReporterId(value))
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.HasOne(i => i.Reporter)
                .WithMany(r => r.Incidents)
                .HasForeignKey(i => i.ReporterId)
                .IsRequired();

            builder.Property(i => i.Title)
                .HasConversion(
                    title => title.Title,
                    value => IncidentTitle.Create(value))
                .HasMaxLength(350)
                .IsRequired();

            builder.Property(i => i.Description)
                .HasConversion(
                    description => description.Description,
                    value => IncidentDescription.Create(value))
                .HasMaxLength(500)
                .IsRequired();



            builder.Property(i => i.Environment)
                .HasConversion(
                    env => env.Name,
                    value => CreateEnv(value)
                )
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.Scope)
                .HasConversion(
                    sc => sc.Name,
                    value => CreateScope(value)
                )
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.IncidentSeverity)
                .HasConversion(
                    s => s.Name,
                    value => CreateSeverity(value)
                )
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime2");

            builder.Property(i => i.UpdatedAt)
                .HasColumnType("datetime2");

        }
        public static InfrastructureEnvironment CreateEnv(string name)
        {
            return name switch
            {
                "Developement" => new DevelopmentEnvironment(),
                "Staging" => new StagingEnvironment(),
                "Production" => new ProductionEnvironment(),
                _ => throw new InvalidOperationException("Invalid env")

            };
        }
        public static IncidentScope CreateScope(string name)
        {
            return name switch
            {
                "Organisation" => new OrganizationScope(),
                "Departement" => new DepartmentScope(),
                "SingleUser" => new SingleUserScope(),
                _ => throw new InvalidOperationException("Invalid scope")

            };

        }
        public static IncidentSeverity CreateSeverity(string name)
        {
            return name switch
            {
                "Critical" => new CriticalIncidentSeverity(),
                "High" => new HighIncidentSeverity(),
                "Medium" => new MeduimIncidentSeverity(),
                "Low" => new LowIncidentSeverity(),
                _ => throw new InvalidOperationException("invalid severity")
            };

        }
    }
}
