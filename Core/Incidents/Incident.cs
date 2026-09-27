using Core.Exceptions;
using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;
using Core.Reporters;
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
        public IncidentScope Scope { get; private set; }
        public IncidentSla IncidentSla { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        private Incident() { }  
        private Incident(IncidentTitle title, IncidentDescription description, Reporter reporter, IncidentCategory category, InfrastructureEnvironment environment, IncidentSeverity incidentSeverity,  IncidentScope scope,IncidentSla sla,DateTime CreatedAt) =>
            (Title, Description, Reporter, Category, Environment, IncidentSeverity, Scope,IncidentSla,CreatedAt,UpdatedAt) = 
            (title,description,reporter,category,environment,incidentSeverity,scope,sla,CreatedAt,null);

        public static Incident Create(IncidentTitle title , IncidentDescription description , Reporter reporter , IncidentCategory category,IncidentScope scope , InfrastructureEnvironment env)
        {
            if (title is null) throw new IncidentTitleIsNullException();
            if (description is null) throw new IncidentDescriptionIsNullException();
            if (reporter is null) throw new ReporterIsNullException();
            if (category is null) throw new IncidentCategoryIsNullException();
            if (env is null) throw new IncidentInfrastructureEnvironmentIsNullException();

            var now = DateTime.UtcNow;

            var severity = UpdateSeverity(category, scope, env);
            var sla = UpdateIncidentSla(category, scope, env);

            return new Incident(title,description,reporter,category,env,severity,scope,sla,now);
        }
        public void ChangeTitle(IncidentTitle title)
        {
            if (title == null) throw new IncidentTitleIsNullException();
            Title = title;
        }
        public void ChangeDescription(IncidentDescription description)
        {
            if (description == null) throw new IncidentDescriptionIsNullException();
            Description = description;
        }
        public void ChangeScope(IncidentScope scope)
        {
            if(scope == null) throw new IncidentScopeInNullException(); 
            Scope = scope;
            ChangeSeverity(UpdateSeverity(Category,Scope,Environment));
            ChangeIncidentSla(UpdateIncidentSla(Category,Scope,Environment));
            
        }
        public void ChangeEnvironment(InfrastructureEnvironment env)
        {
            if (env is null) throw new IncidentInfrastructureEnvironmentIsNullException();
            Environment = env;
            ChangeSeverity(UpdateSeverity(Category,Scope,Environment));
            ChangeIncidentSla(UpdateIncidentSla(Category,Scope,Environment));
        }
        private static IncidentSla UpdateIncidentSla(IncidentCategory category , IncidentScope scope , InfrastructureEnvironment env)
        {
            var resolutionDuration = ResolvedWithin(category, scope, env);
            var acknowledgeDuration = AcknowledgeWithin(category, scope, env);
            return IncidentSla.Create(IncidentDeadline.Create(acknowledgeDuration),IncidentDeadline.Create(resolutionDuration));

        }
        private void ChangeIncidentSla(IncidentSla incidentSla) => IncidentSla = incidentSla;
        private void ChangeSeverity(IncidentSeverity severity) => IncidentSeverity = severity;
        private static IncidentSeverity UpdateSeverity(IncidentCategory category , IncidentScope scope , InfrastructureEnvironment environment) => 
           IncidentSeverityExtension.Max([category.GetMinimalSeverityLevel(),scope.GetMinimalSeverityLevel(),environment.GetMinimalSeverityLevel()]);

        private static TimeSpan AcknowledgeWithin( IncidentCategory category , IncidentScope scope , InfrastructureEnvironment environment) => new[] { category.GetAcknowledgeTime(), scope.GetAcknowledgeTime(), environment.GetAcknowledgeTime() }.Min();
        private static TimeSpan ResolvedWithin( IncidentCategory category , IncidentScope scope , InfrastructureEnvironment environment) => new[] { category.GetResolutionTime(), scope.GetResolutionTime(), environment.GetResolutionTime() }.Min();

    }
}
