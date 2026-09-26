namespace Core.Exceptions
{
    public sealed class IncidentTitleException(string message) : DomainException(message); 
    public sealed class IncidentDescriptionException(string message) : DomainException(message);
    public sealed class IncidentTitleIsNullException() : DomainException("Incident title cannot be null");
    public sealed class IncidentTitleProblemIsInvalidException(string problem) : DomainException($"Incident title problem is invalid : {problem}");
    public sealed class IncidentTitleServiceIsInvalidException(string service) : DomainException($"Incident title service is invalid : {service}");
    public sealed class IncidentDescriptionIsNullException() : DomainException("Incident description cannot be null");
    public sealed class IncidentCategoryIsNullException() : DomainException("Incident Category cannot be null");
    public sealed class IncidentScopeInNullException() : DomainException("Incident Scop cannot be null");
    public sealed class IncidentDeadlineInvalidDurationException(TimeSpan duration) : DomainException($"SlaDeadline duration cannot be empty {duration} ");
    public sealed class IncidentSlaDeadlineIsNullException() : DomainException("IncidentDeadline cannot be null");
    public sealed class IncidentInfrastructureEnvironmentIsNullException() : DomainException("Incident Environment cannot be null");
    public sealed class IncidentCategorySeniorityIsNullException() : DomainException("Incident Category seniority cannot be null");
    public sealed class IncidentCategorySpecializationIsNullException() : DomainException("Incident specialization seniority cannot be null");
}
