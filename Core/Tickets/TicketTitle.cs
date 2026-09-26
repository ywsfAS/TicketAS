using Core.Exceptions;

namespace Core.Tickets
{
    public sealed record TicketTitle
    {
        private const int MaxLength = 100; 
        public string Name { get;}

        private TicketTitle(string name) => this.Name = name;

        public static TicketTitle Create(string ticketName)
        {
            if(ticketName is null) throw new TicketNameIsNullException();
            ticketName = ticketName.Trim(); 
            if(!IsValidTicketName(ticketName)) throw new TicketNameValidationException(ticketName) ;

            return new TicketTitle(ticketName);
        }
        private static bool IsValidTicketName(string ticketName) =>
             !String.IsNullOrEmpty(ticketName) && ticketName.Length <= MaxLength;
        public int Length => Name.Length;

    }
}
