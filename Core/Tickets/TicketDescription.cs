using Core.Exceptions;

namespace Core.Tickets
{
    public sealed record TicketDescription
    {
        private const int MaxLength = 2000;
        public string Description { get;}
        private TicketDescription(string description) => this.Description = description;
        public static TicketDescription Create(string description)
        {
            if(description is null) throw new TicketDescriptionIsNullException();
            description = description.Trim();
            if(!IsValidTicketDescription(description)) throw new TicketDescriptionValidationException(description);
            return new TicketDescription(description);
        }

        private static bool IsValidTicketDescription(string ticketDescription) => 
           !String.IsNullOrEmpty(ticketDescription) &&  ticketDescription.Length <= MaxLength;

        public int Length => this.Description.Length;

        public static explicit operator string(TicketDescription ticketDescription) => ticketDescription.Description;

    }
}
