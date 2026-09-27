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
using Core.Tickets.TicketPriotities;
using Core.Users;

namespace Core.Tests.Tickets
{
    public class TicketCreationTests
    {
        private static User AUser(string name, string email, string phone) =>
            User.Create(UserName.Create(name), Email.Create(email), PhoneNumber.Create(phone));

        private static Reporter AReporter() =>
            Reporter.Create(AUser("John Doe", "john@example.com", "+1234567890"));

        private static Agent AnAgent(AgentSeniority seniority, params AgentSpecialization[] specializations)
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

        private static Ticket ATicket(Reporter reporter, Agent agent, Incident incident) =>
            Ticket.Create(
                TicketTitle.Create("VPN issue"),
                TicketDescription.Create("Cannot connect to the VPN from home."),
                reporter, agent, incident, new NormalTicket());

        [Fact]
        public void Create_WithQualifiedAgent_Succeeds()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var qualifiedAgent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            var ticket = ATicket(reporter, qualifiedAgent, incident);

            Assert.Equal(qualifiedAgent, ticket.Agent);
            Assert.Equal("Open", ticket.Lifecycle.Name);
        }

        [Fact]
        public void Create_WithUnqualifiedAgent_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter); 
            var unqualifiedAgent = AnAgent(AgentSeniority.Junior); 

            Assert.Throws<TicketAgentIsNotQualifiedForIncident>(() =>
                Ticket.Create(
                    TicketTitle.Create("VPN issue"),
                    TicketDescription.Create("Cannot connect to the VPN from home."),
                    reporter, unqualifiedAgent, incident, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullTitle_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.Throws<TicketTitleIsNullException>(() =>
                Ticket.Create(null!, TicketDescription.Create("desc"), reporter, agent, incident, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullDescription_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.Throws<TicketDescriptionIsNullException>(() =>
                Ticket.Create(TicketTitle.Create("VPN issue"), null!, reporter, agent, incident, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullReporter_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.Throws<ReporterIsNullException>(() =>
                Ticket.Create(TicketTitle.Create("VPN issue"), TicketDescription.Create("desc"), null!, agent, incident, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullAgent_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);

            Assert.Throws<AgentIsNullException>(() =>
                Ticket.Create(TicketTitle.Create("VPN issue"), TicketDescription.Create("desc"), reporter, null!, incident, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullIncident_Throws()
        {
            var reporter = AReporter();
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.Throws<IncidentIsNullException>(() =>
                Ticket.Create(TicketTitle.Create("VPN issue"), TicketDescription.Create("desc"), reporter, agent, null!, new NormalTicket()));
        }

        [Fact]
        public void Create_WithNullPriority_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.Throws<TicketPriorityIsNullException>(() =>
                Ticket.Create(TicketTitle.Create("VPN issue"), TicketDescription.Create("desc"), reporter, agent, incident, null!));
        }

        [Fact]
        public void Create_StartsWithAConversationContainingTheInitialAgent()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            var ticket = ATicket(reporter, agent, incident);

            Assert.Contains(agent.Id, ticket.Conversation.AgentsIds);
        }

        [Fact]
        public void ChangeTitle_WithValidTitle_Succeeds()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);
            var ticket = ATicket(reporter, agent, incident);
            var newTitle = TicketTitle.Create("VPN still broken");

            ticket.ChangeTitle(newTitle);

            Assert.Equal(newTitle, ticket.Title);
        }

        [Fact]
        public void ChangeTitle_WithNullTitle_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);
            var ticket = ATicket(reporter, agent, incident);

            Assert.Throws<TicketTitleIsNullException>(() => ticket.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeDescription_WithValidDescription_Succeeds()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);
            var ticket = ATicket(reporter, agent, incident);
            var newDescription = TicketDescription.Create("Updated description of the problem.");

            ticket.ChangeDescription(newDescription);

            Assert.Equal(newDescription, ticket.Description);
        }

        [Fact]
        public void ChangeDescription_WithNullDescription_Throws()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);
            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);
            var ticket = ATicket(reporter, agent, incident);

            Assert.Throws<TicketDescriptionIsNullException>(() => ticket.ChangeDescription(null!));
        }
    }
}