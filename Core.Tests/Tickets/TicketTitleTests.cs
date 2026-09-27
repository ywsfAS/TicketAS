using Core.Exceptions;
using Core.Tickets;

namespace Core.Tests.Tickets
{
    public class TicketTitleTests
    {
        [Fact]
        public void Create_WithValidName_Succeeds()
        {
            var title = TicketTitle.Create("VPN not working");

            Assert.Equal("VPN not working", title.Name);
            Assert.Equal(15, title.Length);
        }

        [Fact]
        public void Create_TrimsWhitespace()
        {
            var title = TicketTitle.Create("  VPN not working  ");

            Assert.Equal("VPN not working", title.Name);
        }

        [Fact]
        public void Create_WithNull_Throws()
        {
            Assert.Throws<TicketTitleIsNullException>(() => TicketTitle.Create(null!));
        }

        [Fact]
        public void Create_Empty_Throws()
        {
            Assert.Throws<TicketNameValidationException>(() => TicketTitle.Create(""));
        }

        [Fact]
        public void Create_WhitespaceOnly_Throws()
        {
            Assert.Throws<TicketNameValidationException>(() => TicketTitle.Create("   "));
        }

        [Fact]
        public void Create_LongerThanMaxLength_Throws()
        {
            var tooLong = new string('a', 101);

            Assert.Throws<TicketNameValidationException>(() => TicketTitle.Create(tooLong));
        }

        [Fact]
        public void Create_AtExactlyMaxLength_Succeeds()
        {
            var exactlyMax = new string('a', 100);

            var title = TicketTitle.Create(exactlyMax);

            Assert.Equal(100, title.Length);
        }
    }
}