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
    public sealed class TicketNameIsNull() : DomainException("Ticket Name was null");
    public sealed class TicketDescriptionIsNull() : DomainException("Ticket Description was null");
}
