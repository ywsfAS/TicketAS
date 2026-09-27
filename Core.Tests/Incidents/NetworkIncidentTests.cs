using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Incidents.Categories;
using Core.Incidents.Severity;
using Core.Users;

namespace Core.Tests.Incidents
{
    public class NetworkIncidentTests
    {
        private static Agent AnAgent(AgentSeniority seniority, params AgentSpecialization[] specializations)
        {
            var user = User.Create(
                UserName.Create("Agent Smith"),
                Email.Create("agent@example.com"),
                PhoneNumber.Create("+1987654321"));

            var agent = Agent.Create(user, new ActiveAgentState(), seniority);
            foreach (var specialization in specializations)
                agent.AddSpecialization(specialization);

            return agent;
        }

        [Fact]
        public void Create_WithNullSeniority_Throws()
        {
            Assert.Throws<IncidentCategorySeniorityIsNullException>(() =>
                NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any, null!,
                    new[] { AgentSpecialization.Network }));
        }

        [Fact]
        public void Create_WithNullSpecializations_Throws()
        {
            Assert.Throws<IncidentCategorySpecializationIsNullException>(() =>
                NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any, AgentSeniority.Junior, null!));
        }

        [Fact]
        public void Create_StoresRequiredSpecializationsFromConstructor()
        {
            var category = NetworkIncident.Create(
                NetworkSymptomType.Outage, SpecializationMatchRule.Any, AgentSeniority.Junior,
                new[] { AgentSpecialization.Database });

            Assert.Single(category.RequiredSpecializations);
            Assert.Contains(AgentSpecialization.Database, category.RequiredSpecializations);
        }

        [Theory]
        [InlineData(NetworkSymptomType.Outage, typeof(CriticalIncidentSeverity))]
        [InlineData(NetworkSymptomType.Intermittent, typeof(HighIncidentSeverity))]
        [InlineData(NetworkSymptomType.Latency, typeof(MeduimIncidentSeverity))]
        [InlineData(NetworkSymptomType.PacketLoss, typeof(MeduimIncidentSeverity))]
        [InlineData(NetworkSymptomType.SlowConnection, typeof(LowIncidentSeverity))]
        public void GetMinimalSeverityLevel_MapsSymptomToExpectedSeverity(NetworkSymptomType symptom, Type expectedType)
        {
            var category = NetworkIncident.Create(symptom, SpecializationMatchRule.Any, AgentSeniority.Junior,
                new[] { AgentSpecialization.Network });

            Assert.IsType(expectedType, category.GetMinimalSeverityLevel());
        }

        [Theory]
        [InlineData(NetworkSymptomType.Outage, 15)]
        [InlineData(NetworkSymptomType.Intermittent, 30)]
        [InlineData(NetworkSymptomType.SlowConnection, 240)]
        public void GetAcknowledgeTime_MapsSymptomToExpectedDuration(NetworkSymptomType symptom, int expectedMinutes)
        {
            var category = NetworkIncident.Create(symptom, SpecializationMatchRule.Any, AgentSeniority.Junior,
                new[] { AgentSpecialization.Network });

            Assert.Equal(TimeSpan.FromMinutes(expectedMinutes), category.GetAcknowledgeTime());
        }

        [Theory]
        [InlineData(NetworkSymptomType.Outage, 2)]
        [InlineData(NetworkSymptomType.Intermittent, 4)]
        [InlineData(NetworkSymptomType.SlowConnection, 24)]
        public void GetResolutionTime_MapsSymptomToExpectedDuration(NetworkSymptomType symptom, int expectedHours)
        {
            var category = NetworkIncident.Create(symptom, SpecializationMatchRule.Any, AgentSeniority.Junior,
                new[] { AgentSpecialization.Network });

            Assert.Equal(TimeSpan.FromHours(expectedHours), category.GetResolutionTime());
        }

        [Fact]
        public void IsQualified_AgentBelowRequiredSeniority_ReturnsFalse()
        {
            var category = NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any,
                AgentSeniority.Senior, new[] { AgentSpecialization.Network });

            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);

            Assert.False(category.IsQualified(agent));
        }

        [Fact]
        public void IsQualified_AnyRule_MatchesWhenAgentHasAtLeastOneRequiredSpecialization()
        {
            var category = NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any,
                AgentSeniority.Junior, new[] { AgentSpecialization.Network, AgentSpecialization.Database });

            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Database);

            Assert.True(category.IsQualified(agent));
        }

        [Fact]
        public void IsQualified_AnyRule_FailsWhenAgentHasNoneOfTheRequiredSpecializations()
        {
            var category = NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.Any,
                AgentSeniority.Junior, new[] { AgentSpecialization.Network });

            var agent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Database);

            Assert.False(category.IsQualified(agent));
        }

        [Fact]
        public void IsQualified_AllRule_RequiresEveryRequiredSpecialization()
        {
            var category = NetworkIncident.Create(NetworkSymptomType.Outage, SpecializationMatchRule.All,
                AgentSeniority.Junior, new[] { AgentSpecialization.Network, AgentSpecialization.Database });

            var partiallyQualifiedAgent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network);
            var fullyQualifiedAgent = AnAgent(AgentSeniority.Junior, AgentSpecialization.Network, AgentSpecialization.Database);

            Assert.False(category.IsQualified(partiallyQualifiedAgent));
            Assert.True(category.IsQualified(fullyQualifiedAgent));
        }
    }
}
