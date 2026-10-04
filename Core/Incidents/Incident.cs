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
        public ReporterId ReporterId { get; private set; }

        public IncidentCategory Category { get; private set; }
        public IncidentCategoryId CategoryId { get; private set; }
        public IIncidentCategoryBehavior Behavior { get; private set; } 
        public InfrastructureEnvironment Environment { get; private set; }
        public IncidentSeverity IncidentSeverity { get; private set; }
        public IncidentScope Scope { get; private set; }
        public IncidentSla IncidentSla { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        private Incident() { }  
        private Incident(IncidentTitle title, IncidentDescription description, Reporter reporter, IncidentCategory category,IIncidentCategoryBehavior behavior, InfrastructureEnvironment environment, IncidentSeverity incidentSeverity,  IncidentScope scope,IncidentSla sla,DateTime CreatedAt) =>
            (Title, Description, Reporter, ReporterId, Category, CategoryId, Behavior, Environment, IncidentSeverity, Scope,IncidentSla,CreatedAt,UpdatedAt) =
            (title,description,reporter,reporter.Id,category,category.Id,behavior,environment,incidentSeverity,scope,sla,CreatedAt,null);

        public static Incident Create(IncidentTitle title , IncidentDescription description , Reporter reporter , IncidentCategory category,IIncidentCategoryBehavior behavior,IncidentScope scope , InfrastructureEnvironment env)
        {
            if (title is null) throw new IncidentTitleIsNullException();
            if (description is null) throw new IncidentDescriptionIsNullException();
            if (reporter is null) throw new ReporterIsNullException();
            if (category is null) throw new IncidentCategoryIsNullException();
            if (env is null) throw new IncidentInfrastructureEnvironmentIsNullException();
            if(scope is null) throw new IncidentScopeInNullException();

            var now = DateTime.UtcNow;

            var severity = UpdateSeverity(behavior, scope, env);
            var sla = UpdateIncidentSla(behavior, scope, env);


            var incident = new Incident(title,description,reporter,category,behavior,env,severity,scope,sla,now);
            incident.Id = new IncidentId(Guid.NewGuid());

            return incident;
        }
        public void ChangeTitle(IncidentTitle title)
        {
            if (title == null) throw new IncidentTitleIsNullException();
            Title = title;
            Update();
        }
        public void ChangeDescription(IncidentDescription description)
        {
            if (description == null) throw new IncidentDescriptionIsNullException();
            Description = description;
            Update();
        }
        public void ChangeScope(IncidentScope scope)
        {
            if(scope == null) throw new IncidentScopeInNullException(); 
            Scope = scope;
            ChangeSeverity(UpdateSeverity(Behavior,Scope,Environment));
            ChangeIncidentSla(UpdateIncidentSla(Behavior,Scope,Environment));
            Update();
            
        }
        public void ChangeScope(IncidentScope scope, IIncidentCategoryBehavior behavior)
        {
            Behavior = behavior;
            ChangeScope(scope);
        }
        public void ChangeEnvironment(InfrastructureEnvironment env)
        {
            if (env is null) throw new IncidentInfrastructureEnvironmentIsNullException();
            Environment = env;
            ChangeSeverity(UpdateSeverity(Behavior,Scope,Environment));
            ChangeIncidentSla(UpdateIncidentSla(Behavior,Scope,Environment));
            Update();
        }
        public void ChangeEnvironment(InfrastructureEnvironment env, IIncidentCategoryBehavior behavior)
        {
            Behavior = behavior;
            ChangeEnvironment(env);
        }
        private static IncidentSla UpdateIncidentSla(IIncidentCategoryBehavior category , IncidentScope scope , InfrastructureEnvironment env)
        {
            var resolutionDuration = ResolvedWithin(category, scope, env);
            var acknowledgeDuration = AcknowledgeWithin(category, scope, env);
            return IncidentSla.Create(IncidentDeadline.Create(acknowledgeDuration),IncidentDeadline.Create(resolutionDuration));

        }
        private void ChangeIncidentSla(IncidentSla incidentSla) => IncidentSla = incidentSla;
        private void ChangeSeverity(IncidentSeverity severity) => IncidentSeverity = severity;
        private void Update() => UpdatedAt = DateTime.UtcNow;
        private static IncidentSeverity UpdateSeverity(IIncidentCategoryBehavior category , IncidentScope scope , InfrastructureEnvironment environment) => 
           IncidentSeverityExtension.Max([category.GetMinimalSeverityLevel(),scope.GetMinimalSeverityLevel(),environment.GetMinimalSeverityLevel()]);

        private static TimeSpan AcknowledgeWithin( IIncidentCategoryBehavior category , IncidentScope scope , InfrastructureEnvironment environment) => new[] { category.GetAcknowledgeTime(), scope.GetAcknowledgeTime(), environment.GetAcknowledgeTime() }.Min();
        private static TimeSpan ResolvedWithin( IIncidentCategoryBehavior category , IncidentScope scope , InfrastructureEnvironment environment) => new[] { category.GetResolutionTime(), scope.GetResolutionTime(), environment.GetResolutionTime() }.Min();

    }
}
