using Core.Exceptions;
using Core.Utilities;

namespace Core
{
    public sealed record TicketId(Guid Value) : StrongTypedId(Value);
    public sealed record TicketName
    {
        private const int MaxLength = 100; 
        public string Name { get;}

        private TicketName(string name) => this.Name = name;

        public static TicketName Create(string ticketName)
        {
            if(ticketName is null) throw new TicketNameIsNull();
            ticketName = ticketName.Trim(); 
            if(!IsValidTicketName(ticketName)) throw new TicketNameValidationException(ticketName) ;

            return new TicketName(ticketName);
        }
        private static bool IsValidTicketName(string ticketName) =>
             !String.IsNullOrEmpty(ticketName) && ticketName.Length <= MaxLength;
        public int Length => Name.Length;

    }

    public sealed record TicketDescription
    {
        private const int MaxLength = 2000;
        public string Description { get;}
        private TicketDescription(string description) => this.Description = description;
        public static TicketDescription Create(string description)
        {
            if(description is null) throw new TicketDescriptionIsNull();
            description = description.Trim();
            if(!IsValidTicketDescription(description)) throw new TicketDescriptionValidationException(description);
            return new TicketDescription(description);
        }

        private static bool IsValidTicketDescription(string ticketDescription) => 
           !String.IsNullOrEmpty(ticketDescription) &&  ticketDescription.Length <= MaxLength;

        public int Length => this.Description.Length;

        public static explicit operator string(TicketDescription ticketDescription) => ticketDescription.Description;

    }
    public abstract class TicketPriority { }
    public sealed class CriticalTicket : TicketPriority { }
    public sealed class NormalTicket : TicketPriority { }
    public sealed class LowTicket : TicketPriority { }

    public sealed class Ticket : Entity<TicketId>
    {
        public TicketName Name { get; private set; }
        public TicketDescription Description { get; private set; }
        public Reporter Reporter { get; private set; }
        public TicketPriority Priority { get; private set; }

        public TicketCategory Category { get; private set; }

        public DateTime CreatedAt;
        public DateTime? UpdatedAt;
        
    }
}
