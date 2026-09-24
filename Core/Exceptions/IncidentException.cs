namespace Core.Exceptions
{
    public sealed class IncidentTitleException(string message) : DomainException(message); 
    public sealed class IncidentDescriptionException(string message) : DomainException(message);
    public sealed class IncidentTitleIsNullException() : DomainException("Incident title cannot be null");
    public sealed class IncidentTitleProblemIsInvalidException(string problem) : DomainException($"Incident title problem is invalid : {problem}");
    public sealed class IncidentTitleServiceIsInvalidException(string service) : DomainException($"Incident title service is invalid : {service}");
    public sealed class IncidentDescriptionIsNullException() : DomainException("Incident description cannot be null");
    public sealed class IncidentCategoryIsNullException() : DomainException("Incident Category cannot be null");
}
