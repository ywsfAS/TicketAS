using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;

namespace Application.Incidents;

public static class IncidentDomainLookups
{
    public static IncidentScope? Scope(string? name) => name switch
    {
        "SingleUser" => new SingleUserScope(),
        "Department" or "Departement" => new DepartmentScope(),
        "Organization" => new OrganizationScope(),
        _ => null
    };

    public static InfrastructureEnvironment? Environment(string? name) => name switch
    {
        "Development" => new DevelopmentEnvironment(),
        "Staging" => new StagingEnvironment(),
        "Production" => new ProductionEnvironment(),
        _ => null
    };

    public static IncidentSeverity? Severity(string? name) => name switch
    {
        "Low" => new LowIncidentSeverity(),
        "Medium" or "Meduim" => new MeduimIncidentSeverity(),
        "High" => new HighIncidentSeverity(),
        "Critical" => new CriticalIncidentSeverity(),
        _ => null
    };

}
