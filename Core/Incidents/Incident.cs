using Core.Exceptions;
using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;
using Core.Utilities;
namespace Core.Incidents
{
    public class Incident : Entity<IncidentId>
    {
        public IncidentTitle Title { get; private set; }
        public IncidentDescription Description { get; private set; }    
        public Reporter Reporter { get; private set; }
        public IncidentCategory Category { get; private set; }
        public InfrastructureEnvironment Environment { get; private set; }
        public IncidentSeverity IncidentSeverity { get; private set; }
        public IncidentInfos IncidentInfos { get; private set; }
        public IncidentScope Scope { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Incident() { }  

        public static Incident Create(IncidentTitle title , IncidentDescription description , Reporter reporter , IncidentCategory category,IncidentScope scope , InfrastructureEnvironment env)
        {
            if (title is null) throw new IncidentTitleIsNullException();
            if (description is null) throw new IncidentDescriptionIsNullException();
            if (reporter is null) throw new ReporterIsNullException();
            if (category is null) throw new IncidentCategoryIsNullException();

            IncidentSeverity severity = IncidentSeverityExtension.Max([category.GetMinimalSeverityLevel(),scope.GetMinimalSeverityLevel(),env.GetMinimalSeverityLevel()]);

            return new Incident
            {
                Title = title,
                Description = description,
                Reporter = reporter,
                Category = category,
                Scope = scope,
                IncidentInfos = new IncidentInfos(),
                Environment = env,
                IncidentSeverity = severity,
                CreatedAt = DateTime.Now,
                UpdatedAt = null

            };

        }
        public void ChangeTitle(IncidentTitle title)
        {
            if (Title == null) throw new IncidentTitleIsNullException();
            Title = title;
        }
        public void ChangeDescription(IncidentDescription description)
        {
            if (Description == null) throw new IncidentDescriptionIsNullException();
            Description = description;
        }

    }
}
