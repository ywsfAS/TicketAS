using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Reporters;
using Core.Tickets;
using Core.Tickets.Messages;
using Core.Tickets.TicketPriotities;
using Core.Users;

namespace Core.Tests.Tickets
{
    public class TicketLifecycleTests
    {
        private static User AUser(string name, string email, string phone) =>
            User.Create(UserName.Create(name), Email.Create(email), PhoneNumber.Create(phone));

        private static Reporter AReporter() =>
            Reporter.Create(AUser("John Doe", "john@example.com", "+1234567890"));

        private static Agent AnAgent() =>
            AnAgentWith(AgentSeniority.Junior, AgentSpecialization.Network);

        private static Agent AnAgentWith(AgentSeniority seniority, params AgentSpecialization[] specializations)
        {
            var agent = Agent.Create(AUser("Agent Smith", "agent@example.com", "+1987654321"), new ActiveAgentState(), seniority);
            foreach (var specialization in specializations)
                agent.AddSpecialization(specialization);

            return agent;
        }

        private static Incident AnIncident(Reporter reporter) =>
            Incident.Create(
                IncidentTitle.Create("Outage", "Payments"),
                IncidentDescription.Create("Users cannot process payments right now."),
                reporter,
                NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any, AgentSeniority.Junior,
                    new[] { AgentSpecialization.Network }),
                new SingleUserScope(),
                new ProductionEnvironment());

        private static MessageContent AMessage() =>
            MessageContent.Create(MessageTitle.Create("Update"), MessageBody.Create("This is a message body."));

        private static Ticket ATicket()
        {
            var reporter = AReporter();
            return Ticket.Create(
                TicketTitle.Create("VPN not working"),
                TicketDescription.Create("The VPN client fails to connect from the home network."),
                reporter, AnAgent(), AnIncident(reporter), new NormalTicket());
        }

        [Fact]
        public void NewTicket_IsOpen()
        {
            var ticket = ATicket();

            Assert.Equal("Open", ticket.Lifecycle.Name);
        }

        [Fact]
        public void Open_StartWork_MovesToInProgress()
        {
            var ticket = ATicket();

            ticket.StartWork();

            Assert.Equal("InProgress", ticket.Lifecycle.Name);
        }

        [Theory]
        [InlineData("Resolve")]
        [InlineData("Close")]
        [InlineData("Reopen")]
        public void Open_DisallowedActions_Throw(string action)
        {
            var ticket = ATicket();

            Action act = action switch
            {
                "Resolve" => ticket.Resolve,
                "Close" => ticket.Close,
                "Reopen" => ticket.Reopen,
                _ => throw new InvalidOperationException()
            };

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(act);
        }

        [Fact]
        public void Open_ChangePriority_Succeeds()
        {
            var ticket = ATicket();

            ticket.ChangePriority(new CriticalTicket());

            Assert.IsType<CriticalTicket>(ticket.Priority);
        }

        [Fact]
        public void Open_ReporterSends_AddsMessageToConversation()
        {
            var ticket = ATicket();

            ticket.ReporterSends(AMessage());

            Assert.Single(ticket.Conversation.Messages);
        }

        [Fact]
        public void Open_AgentSends_AddsMessageToConversation()
        {
            var ticket = ATicket();

            ticket.AgentSends(AMessage());

            Assert.Single(ticket.Conversation.Messages);
        }

        [Fact]
        public void InProgress_Resolve_MovesToResolved()
        {
            var ticket = ATicket();
            ticket.StartWork();

            ticket.Resolve();

            Assert.Equal("Resolved", ticket.Lifecycle.Name);
        }

        [Theory]
        [InlineData("StartWork")]
        [InlineData("Close")]
        [InlineData("Reopen")]
        public void InProgress_DisallowedActions_Throw(string action)
        {
            var ticket = ATicket();
            ticket.StartWork();

            Action act = action switch
            {
                "StartWork" => ticket.StartWork,
                "Close" => ticket.Close,
                "Reopen" => ticket.Reopen,
                _ => throw new InvalidOperationException()
            };

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(act);
        }

        [Fact]
        public void InProgress_ChangePriority_Succeeds()
        {
            var ticket = ATicket();
            ticket.StartWork();

            ticket.ChangePriority(new HighTicket());

            Assert.IsType<HighTicket>(ticket.Priority);
        }

        [Fact]
        public void Resolved_Close_MovesToClosed()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();

            ticket.Close();

            Assert.Equal("Closed", ticket.Lifecycle.Name);
        }

        [Fact]
        public void Resolved_Reopen_MovesBackToInProgress()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();

            ticket.Reopen();

            Assert.Equal("InProgress", ticket.Lifecycle.Name);
        }

        [Fact]
        public void Resolved_ReporterSends_AutomaticallyReopensToInProgress()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();

            ticket.ReporterSends(AMessage());

            Assert.Equal("InProgress", ticket.Lifecycle.Name);
            Assert.Single(ticket.Conversation.Messages);
        }

        [Fact]
        public void Resolved_AgentSends_DoesNotChangeLifecycle()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();

            ticket.AgentSends(AMessage());

            Assert.Equal("Resolved", ticket.Lifecycle.Name);
        }

        [Theory]
        [InlineData("StartWork")]
        [InlineData("Resolve")]
        [InlineData("ChangePriority")]
        public void Resolved_DisallowedActions_Throw(string action)
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();

            Action act = action switch
            {
                "StartWork" => ticket.StartWork,
                "Resolve" => ticket.Resolve,
                "ChangePriority" => () => ticket.ChangePriority(new LowTicket()),
                _ => throw new InvalidOperationException()
            };

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(act);
        }

        [Theory]
        [InlineData("StartWork")]
        [InlineData("Resolve")]
        [InlineData("Reopen")]
        [InlineData("ChangePriority")]
        public void Closed_EverythingIsRejected(string action)
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();
            ticket.Close();

            Action act = action switch
            {
                "StartWork" => ticket.StartWork,
                "Resolve" => ticket.Resolve,
                "Reopen" => ticket.Reopen,
                "ChangePriority" => () => ticket.ChangePriority(new LowTicket()),
                _ => throw new InvalidOperationException()
            };

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(act);
        }

        [Fact]
        public void Closed_ReporterSends_Throws()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();
            ticket.Close();

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(() => ticket.ReporterSends(AMessage()));
        }

        [Fact]
        public void Closed_AgentSends_Throws()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();
            ticket.Close();

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(() => ticket.AgentSends(AMessage()));
        }

        [Fact]
        public void Closed_AssignAgent_Throws()
        {
            var ticket = ATicket();
            ticket.StartWork();
            ticket.Resolve();
            ticket.Close();
            var newAgent = AnAgent();

            Assert.Throws<TicketInvalidActionWithinLifecycleException>(() => ticket.AssignAgent(newAgent));
        }


        [Fact]
        public void AssignAgent_WhileOpen_AddsNewAgentAsConversationParticipant()
        {
            var ticket = ATicket();
            var newAgent = AnAgent();

            ticket.AssignAgent(newAgent);

            Assert.Contains(newAgent.Id, ticket.Conversation.AgentsIds);
            ticket.AgentSends(AMessage());
            Assert.Single(ticket.Conversation.Messages);
        }

        [Fact]
        public void AssignAgent_WhileInProgress_AddsNewAgentAsConversationParticipant()
        {
            var ticket = ATicket();
            ticket.StartWork();
            var newAgent = AnAgent();

            ticket.AssignAgent(newAgent);

            Assert.Contains(newAgent.Id, ticket.Conversation.AgentsIds);
            ticket.AgentSends(AMessage());
            Assert.Single(ticket.Conversation.Messages);
        }

        [Fact]
        public void AssignAgent_ReassignmentDoesNotRemoveThePreviousAgentAsParticipant()
        {
            // Option 2 semantics: conversation history/participants accumulate, they never
            // get replaced when the ticket is reassigned to someone new.
            var ticket = ATicket();
            var originalAgentId = ticket.Agent.Id;
            var newAgent = AnAgent();

            ticket.AssignAgent(newAgent);

            Assert.Contains(originalAgentId, ticket.Conversation.AgentsIds);
            Assert.Contains(newAgent.Id, ticket.Conversation.AgentsIds);
        }

        [Fact]
        public void AssignAgent_WithUnqualifiedAgent_Throws()
        {
            var ticket = ATicket();
            var unqualifiedAgent = AnAgentWith(AgentSeniority.Junior); 
            Assert.Throws<TicketAgentIsNotQualifiedForIncident>(() => ticket.AssignAgent(unqualifiedAgent));
        }

        [Fact]
        public void AssignAgent_UpdatesTheCurrentAgentPointer()
        {
            var ticket = ATicket();
            var newAgent = AnAgent();

            ticket.AssignAgent(newAgent);

            Assert.Equal(newAgent, ticket.Agent);
        }
    }
}