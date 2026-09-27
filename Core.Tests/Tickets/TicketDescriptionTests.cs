using Core.Exceptions;
using Core.Tickets;

namespace Core.Tests.Tickets
{
    public class TicketDescriptionTests
    {
        [Fact]
        public void Create_WithValidDescription_Succeeds()
        {
            var description = TicketDescription.Create("The VPN client fails to connect from home.");

            Assert.Equal("The VPN client fails to connect from home.", description.Description);
        }

        [Fact]
        public void Create_TrimsWhitespace()
        {
            var description = TicketDescription.Create("   Some description here.   ");

            Assert.Equal("Some description here.", description.Description);
        }

        [Fact]
        public void Create_WithNull_Throws()
        {
            Assert.Throws<TicketDescriptionIsNullException>(() => TicketDescription.Create(null!));
        }

        [Fact]
        public void Create_Empty_Throws()
        {
            Assert.Throws<TicketDescriptionValidationException>(() => TicketDescription.Create(""));
        }

        [Fact]
        public void Create_LongerThanMaxLength_Throws()
        {
            var tooLong = new string('a', 2001);

            Assert.Throws<TicketDescriptionValidationException>(() => TicketDescription.Create(tooLong));
        }

        [Fact]
        public void Create_AtExactlyMaxLength_Succeeds()
        {
            var exactlyMax = new string('a', 2000);

            var description = TicketDescription.Create(exactlyMax);

            Assert.Equal(2000, description.Length);
        }

        [Fact]
        public void ExplicitStringConversion_ReturnsTheDescriptionText()
        {
            var description = TicketDescription.Create("Some description here.");

            var asString = (string)description;

            Assert.Equal("Some description here.", asString);
        }
    }
}