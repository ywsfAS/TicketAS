using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using Core.Exceptions;
using Core.Reporters;
using Core.Tickets.Conversation;
using Core.Tickets.Messages;
using Core.Users;

namespace Core.Tests.Tickets
{
    public class ConversationTests
    {
        private static User AUser(string name, string email, string phone) =>
            User.Create(UserName.Create(name), Email.Create(email), PhoneNumber.Create(phone));

        private static Reporter AReporter() =>
            Reporter.Create(AUser("John Doe", "john@example.com", "+1234567890"));

        private static Agent AnAgent() =>
            Agent.Create(AUser("Agent Smith", "agent@example.com", "+1987654321"), new ActiveAgentState(), AgentSeniority.Mid);

        private static MessageContent AMessage() =>
            MessageContent.Create(MessageTitle.Create("Update"), MessageBody.Create("This is a message body."));

        [Fact]
        public void Create_WithNullReporter_Throws()
        {
            Assert.Throws<TicketConversationReporterIsNullException>(() => Conversation.Create(null!, AnAgent()));
        }

        [Fact]
        public void Create_WithNullInitialAgent_Throws()
        {
            Assert.Throws<TicketConversationAgentIsNullException>(() => Conversation.Create(AReporter(), null!));
        }

        [Fact]
        public void Create_AddsTheInitialAgentAsAParticipant()
        {
            var agent = AnAgent();

            var conversation = Conversation.Create(AReporter(), agent);

            Assert.Contains(agent.Id, conversation.AgentsIds);
        }

        [Fact]
        public void ReporterSends_AddsAMessageAttributedToTheReporter()
        {
            var reporter = AReporter();
            var conversation = Conversation.Create(reporter, AnAgent());

            conversation.ReporterSends(AMessage());

            var message = Assert.Single(conversation.Messages);
            Assert.Equal(reporter.Id, message.ParticipantId);
        }

        [Fact]
        public void ReporterSends_WithNullContent_Throws()
        {
            var conversation = Conversation.Create(AReporter(), AnAgent());

            Assert.Throws<TicketMessageContentIsNullException>(() => conversation.ReporterSends(null!));
        }

        [Fact]
        public void AgentSends_ByAParticipant_AddsAMessageAttributedToThatAgent()
        {
            var agent = AnAgent();
            var conversation = Conversation.Create(AReporter(), agent);

            conversation.AgentSends(agent, AMessage());

            var message = Assert.Single(conversation.Messages);
            Assert.Equal(agent.Id, message.ParticipantId);
        }

        [Fact]
        public void AgentSends_WithNullContent_Throws()
        {
            var agent = AnAgent();
            var conversation = Conversation.Create(AReporter(), agent);

            Assert.Throws<TicketMessageContentIsNullException>(() => conversation.AgentSends(agent, null!));
        }

        [Fact]
        public void AddParticipant_ThenThatAgentCanSendMessages()
        {
            var conversation = Conversation.Create(AReporter(), AnAgent());
            var newAgent = AnAgent();

            conversation.AgentSends(newAgent, AMessage());
            Assert.Single(conversation.Messages);
        }

        [Fact]
        public void AddParticipant_WithNullAgent_Throws()
        {
            var conversation = Conversation.Create(AReporter(), AnAgent());

            Assert.Throws<TicketConversationAgentIsNullException>(() => conversation.AgentSends(null!,AMessage()));
        }

        [Fact]
        public void AddParticipant_SameAgentTwice_DoesNotDuplicate()
        {
            var agent = AnAgent();
            var conversation = Conversation.Create(AReporter(), agent);

            conversation.AgentSends(agent, AMessage());

            Assert.Single(conversation.AgentsIds);
        }

    }
}