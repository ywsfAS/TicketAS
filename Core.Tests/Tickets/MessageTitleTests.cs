using Core.Agents;
using Core.Exceptions;
using Core.Tickets.Messages;

namespace Core.Tests.Tickets
{
    public class MessageTitleTests
    {
        [Fact]
        public void Create_WithValidTitle_Succeeds()
        {
            var title = MessageTitle.Create("Investigating now");

            Assert.Equal("Investigating now", title.Title);
        }

        [Fact]
        public void Create_ShorterThanMinLength_Throws()
        {
            Assert.Throws<TicketMessageTitleInvalidException>(() => MessageTitle.Create("Hi"));
        }

        [Fact]
        public void Create_WithNoAlphanumericCharacters_Throws()
        {
            Assert.Throws<TicketMessageTitleInvalidException>(() => MessageTitle.Create("!!!!!"));
        }

        [Fact]
        public void Create_WithNull_Throws()
        {
            Assert.Throws<TicketMessageTitleIsNullException>(() => MessageTitle.Create(null!));
        }
        [Fact]
        public void Create_AtExactlyMinLength_Succeeds()
        {
            var title = MessageTitle.Create("abc"); // MinLength = 3

            Assert.Equal("abc", title.Title);
        }
    }

    public class MessageBodyTests
    {
        [Fact]
        public void Create_WithValidBody_Succeeds()
        {
            var body = MessageBody.Create("We found the root cause and are deploying a fix.");

            Assert.Equal("We found the root cause and are deploying a fix.", body.Body);
        }

        [Fact]
        public void Create_Empty_Throws()
        {
            Assert.Throws<TicketMessageBodyInvalidException>(() => MessageBody.Create(""));
        }

        [Fact]
        public void Create_WithNull_Throws()
        {
            Assert.Throws<TicketMessageBodyIsNullException>(() => MessageBody.Create(null!));
        }

        [Fact]
        public void Create_WithNoAlphanumericCharacters_Throws()
        {
            Assert.Throws<TicketMessageBodyInvalidException>(() => MessageBody.Create("???"));
        }
    }

    public class MessageContentTests
    {
        [Fact]
        public void Create_WithNullTitle_Throws()
        {
            Assert.Throws<TicketMessageTitleIsNullException>(
                () => MessageContent.Create(null!, MessageBody.Create("valid body")));
        }

        [Fact]
        public void Create_WithNullBody_Throws()
        {
            Assert.Throws<TicketMessageBodyIsNullException>(
                () => MessageContent.Create(MessageTitle.Create("valid title"), null!));
        }

        [Fact]
        public void Create_WithValidTitleAndBody_Succeeds()
        {
            var title = MessageTitle.Create("Update");
            var body = MessageBody.Create("Still investigating.");

            var content = MessageContent.Create(title, body);

            Assert.Equal(title, content.Title);
            Assert.Equal(body, content.Body);
        }
    }

    public class MessageEntityTests
    {
        [Fact]
        public void Create_WithValidContent_Succeeds()
        {
            var participantId = new AgentId(Guid.NewGuid());
            var content = MessageContent.Create(MessageTitle.Create("Update"), MessageBody.Create("Still investigating."));
            var sentAt = DateTime.UtcNow;

            var message = Message.Create(participantId, content, sentAt);

            Assert.Equal(participantId, message.ParticipantId);
            Assert.Equal(content, message.Content);
            Assert.Equal(sentAt, message.SentAt);
        }

        [Fact]
        public void Create_WithNullContent_Throws()
        {
            var participantId = new AgentId(Guid.NewGuid());

            Assert.Throws<TicketMessageContentIsNullException>(
                () => Message.Create(participantId, null!, DateTime.UtcNow));
        }
    }
}