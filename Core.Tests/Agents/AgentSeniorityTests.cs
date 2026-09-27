using Core.Agents.Seniority;

namespace Core.Tests.Agents
{
    public class AgentSeniorityTests
    {
        [Fact]
        public void Junior_HasNameJunior()
        {
            Assert.Equal("Junior", AgentSeniority.Junior.Name);
        }

        [Fact]
        public void Mid_HasNameMid() => Assert.Equal("Mid", AgentSeniority.Mid.Name);

        [Fact]
        public void Senior_HasNameSenior() => Assert.Equal("Senior", AgentSeniority.Senior.Name);

        [Theory]
        [InlineData("Junior", "Junior")]
        [InlineData("Mid", "Mid")]
        [InlineData("Senior", "Senior")]
        public void FindByName_WithKnownName_ReturnsMatchingLevel(string name, string expectedName)
        {
            var result = AgentSeniority.FindByName(name);

            Assert.NotNull(result);
            Assert.Equal(expectedName, result!.Name);
        }

        [Fact]
        public void FindByName_WithUnknownName_ReturnsNull()
        {
            Assert.Null(AgentSeniority.FindByName("Principal"));
        }

        [Fact]
        public void Junior_DoesNotMeetMid()
        {
            Assert.False(AgentSeniority.Junior.Meets(AgentSeniority.Mid));
        }

        [Fact]
        public void Senior_MeetsMidAndJunior()
        {
            Assert.True(AgentSeniority.Senior.Meets(AgentSeniority.Mid));
            Assert.True(AgentSeniority.Senior.Meets(AgentSeniority.Junior));
        }

        [Fact]
        public void AnyLevel_MeetsItself()
        {
            Assert.True(AgentSeniority.Mid.Meets(AgentSeniority.Mid));
        }

        [Fact]
        public void CompareTo_OrdersLevelsByRank()
        {
            Assert.True(AgentSeniority.Junior.CompareTo(AgentSeniority.Mid) < 0);
            Assert.True(AgentSeniority.Senior.CompareTo(AgentSeniority.Mid) > 0);
            Assert.Equal(0, AgentSeniority.Mid.CompareTo(AgentSeniority.Mid));
        }
    }
}
