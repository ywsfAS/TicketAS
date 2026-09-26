
namespace Core.Exceptions
{
    public sealed class TicketNameValidationException : DomainException
    {
        public string Name { get; }
        public TicketNameValidationException(string name) : base($"Ticket name was invalid : {name}") { 
            this.Name = name; 
        }
    }
    public sealed class TicketDescriptionValidationException : DomainException
    {
        public string description { get; }

        public TicketDescriptionValidationException(string description) : base($"Ticket description was invalid : {description}")
        {
            this.description = description;
        }
    }
    public sealed class TicketTitleIsNullException() : DomainException("Ticket Name cannot null");
    public sealed class TicketDescriptionIsNullException() : DomainException("Ticket Description cannot null");
    public sealed class TicketMessageIsNullException() : DomainException("Ticket message cannot null");
    public sealed class TicketMessageTitleIsNullException() : DomainException("Ticket message title cannot be null");
    public sealed class TicketMessageTitleInvalidException(string title) : DomainException($"Ticket message title is invalid : {title}");
    public sealed class TicketMessageBodyIsNullException() : DomainException("Ticket message body cannot be null");
    public sealed class TicketMessageBodyInvalidException(string body) : DomainException($"Ticket message body is invalid : {body}");
    public sealed class TicketMessageContentIsNullException() : DomainException("Ticket content cannot be null");
    public sealed class TicketConversationReporterIsNullException() : DomainException("Ticket Report cannot be null in a conversation");
    public sealed class TicketConversationAgentIsNullException() : DomainException("Ticket Agent cannot be null in a conversation");
    public sealed class TicketPriorityIsNullException() : DomainException("Ticket priority cannot be null");
    public sealed class TicketConversationIsNullException() : DomainException("Ticket conversation cannot be null");
    public sealed class TicketAgentIsNotQualifiedForIncident() : DomainException("Ticket's agent is not qualified for incident");
    public sealed class TicketInvalidActionWithinLifecycleException(string action , string lifecycleName) : DomainException( $"Cannot {action} while ticket is {lifecycleName}.");
    public sealed class TicketLifecyleIsNullException() : DomainException("Ticket's lifecycle cannot be null");
    public sealed class TicketConversationAgentNotParticipantException() : DomainException("Ticket agent is not a participant in the conversation");
}
