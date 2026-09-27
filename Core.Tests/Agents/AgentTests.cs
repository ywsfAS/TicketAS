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
using Core.Users;

namespace Core.Tests.Agents
{
    public class AgentTests
    {
        private static User AUser(string name = "Agent Smith", string email = "agent@example.com", string phone = "+1987654321") =>
            User.Create(UserName.Create(name), Email.Create(email), PhoneNumber.Create(phone));

        private static Agent AnAgent(AgentSeniority? seniority = null, params AgentSpecialization[] specializations)
        {
            var agent = Agent.Create(AUser(), new ActiveAgentState(), seniority ?? AgentSeniority.Mid);
            foreach (var specialization in specializations)
                agent.AddSpecialization(specialization);

            return agent;
        }

        private static Incident AnIncident()
        {
            var reporter = Reporter.Create(AUser("John Doe", "john@example.com", "+1234567890"));

            return Incident.Create(
                IncidentTitle.Create("Outage", "Payments"),
                IncidentDescription.Create("Users cannot process payments right now."),
                reporter,
                NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any, AgentSeniority.Junior,
                    new[] { AgentSpecialization.Network }),
                new SingleUserScope(),
                new ProductionEnvironment());
        }

        // ----- Creation -----

        [Fact]
        public void Create_WithNullUser_Throws()
        {
            Assert.Throws<UserIsNullException>(() =>
                Agent.Create(null!, new ActiveAgentState(), AgentSeniority.Mid));
        }

        [Fact]
        public void Create_WithNullState_Throws()
        {
            Assert.Throws<AgentStateIsNullException>(() =>
                Agent.Create(AUser(), null!, AgentSeniority.Mid));
        }

        [Fact]
        public void Create_WithNullSeniority_Throws()
        {
            // Regression test: seniority was previously not a constructor parameter at all,
            // so MeetsSeniority() would NullReferenceException later instead of failing fast here.
            Assert.Throws<AgentSeniorityIsNullException>(() =>
                Agent.Create(AUser(), new ActiveAgentState(), null!));
        }

        [Fact]
        public void Create_WithValidArguments_AssignsSeniorityAndIsActiveWithNoSpecializations()
        {
            var agent = Agent.Create(AUser(), new ActiveAgentState(), AgentSeniority.Senior);

            Assert.Equal(AgentSeniority.Senior, agent.Seniority);
            Assert.Equal("Active", agent.State.Name);
            Assert.Empty(agent.Specializations);
            Assert.Empty(agent.Incidents);
        }

        // ----- Specializations -----

        [Fact]
        public void AddSpecialization_ThenHasSpecialization_ReturnsTrue()
        {
            var agent = AnAgent();

            agent.AddSpecialization(AgentSpecialization.Network);

            Assert.True(agent.HasSpecialization(AgentSpecialization.Network));
            Assert.False(agent.HasSpecialization(AgentSpecialization.Database));
        }

        [Fact]
        public void AddSpecialization_Twice_DoesNotDuplicate()
        {
            var agent = AnAgent();

            agent.AddSpecialization(AgentSpecialization.Network);
            agent.AddSpecialization(AgentSpecialization.Network);

            Assert.Single(agent.Specializations);
        }

        [Fact]
        public void RemoveSpecialization_RemovesIt()
        {
            var agent = AnAgent(AgentSeniority.Mid, AgentSpecialization.Network);

            agent.RemoveSpecialization(AgentSpecialization.Network);

            Assert.False(agent.HasSpecialization(AgentSpecialization.Network));
            Assert.Empty(agent.Specializations);
        }

        [Fact]
        public void RemoveSpecialization_NotPresent_DoesNothing()
        {
            var agent = AnAgent(AgentSeniority.Mid, AgentSpecialization.Network);

            agent.RemoveSpecialization(AgentSpecialization.Database);

            Assert.True(agent.HasSpecialization(AgentSpecialization.Network));
        }

        // ----- Seniority -----

        [Theory]
        [InlineData("Senior", "Mid", true)]
        [InlineData("Senior", "Junior", true)]
        [InlineData("Mid", "Senior", false)]
        [InlineData("Junior", "Mid", false)]
        [InlineData("Mid", "Mid", true)]
        public void MeetsSeniority_ComparesAgainstRequiredLevel(string agentLevel, string requiredLevel, bool expected)
        {
            var agent = AnAgent(AgentSeniority.FindByName(agentLevel));

            var result = agent.MeetsSeniority(AgentSeniority.FindByName(requiredLevel)!);

            Assert.Equal(expected, result);
        }

        // ----- AssignIncident, state-guarded -----

        [Fact]
        public void AssignIncident_WhileActive_Succeeds()
        {
            var agent = AnAgent();
            var incident = AnIncident();

            agent.AssignIncident(incident);

            Assert.Contains(incident, agent.Incidents);
        }

        [Fact]
        public void AssignIncident_WhileSuspended_Throws()
        {
            var agent = AnAgent();
            agent.Suspend();

            Assert.Throws<AgentInvalidActionForStateException>(() => agent.AssignIncident(AnIncident()));
        }

        [Fact]
        public void AssignIncident_WhileUnavailable_Throws()
        {
            var agent = AnAgent();
            agent.MakeUnavailable();

            Assert.Throws<AgentInvalidActionForStateException>(() => agent.AssignIncident(AnIncident()));
        }

        [Fact]
        public void AssignIncident_DoesNotAddIncidentWhenSuspended()
        {
            var agent = AnAgent();
            agent.Suspend();

            try { 
                agent.AssignIncident(AnIncident()); 
            } catch (AgentInvalidActionForStateException) { }

            Assert.Empty(agent.Incidents);
        }


        [Fact]
        public void Active_CanBecomeUnavailable()
        {
            var agent = AnAgent();

            agent.MakeUnavailable();

            Assert.Equal("Unavailable", agent.State.Name);
        }

        [Fact]
        public void Active_CanSuspend()
        {
            var agent = AnAgent();

            agent.Suspend();

            Assert.Equal("Suspended", agent.State.Name);
        }

        [Fact]
        public void Active_CanActivate_IsANoOpTransition()
        {
            var agent = AnAgent();

            agent.Activate();

            Assert.Equal("Active", agent.State.Name);
        }

        [Fact]
        public void Suspended_CanActivate()
        {
            var agent = AnAgent();
            agent.Suspend();

            agent.Activate();

            Assert.Equal("Active", agent.State.Name);
        }

        [Fact]
        public void Suspended_CannotBecomeUnavailable()
        {
            var agent = AnAgent();
            agent.Suspend();

            Assert.Throws<AgentInvalidStateTransitionException>(() => agent.MakeUnavailable());
        }

        [Fact]
        public void Unavailable_CanActivate()
        {
            var agent = AnAgent();
            agent.MakeUnavailable();

            agent.Activate();

            Assert.Equal("Active", agent.State.Name);
        }

        [Fact]
        public void Unavailable_CanSuspend()
        {
            var agent = AnAgent();
            agent.MakeUnavailable();

            agent.Suspend();

            Assert.Equal("Suspended", agent.State.Name);
        }

        [Fact]
        public void Unavailable_CannotBecomeUnavailableAgain()
        {
            var agent = AnAgent();
            agent.MakeUnavailable();

            Assert.Throws<AgentInvalidStateTransitionException>(() => agent.MakeUnavailable());
        }

        [Fact]
        public void Suspend_ThenActivate_AllowsAssignmentAgain()
        {
            var agent = AnAgent();
            agent.Suspend();

            agent.Activate();

            agent.AssignIncident(AnIncident());
            Assert.Single(agent.Incidents);
        }

        [Fact]
        public void MakeUnavailable_ThenActivate_AllowsAssignmentAgain()
        {
            var agent = AnAgent();
            agent.MakeUnavailable();

            agent.Activate();

            agent.AssignIncident(AnIncident());
            Assert.Single(agent.Incidents);
        }
    }
}