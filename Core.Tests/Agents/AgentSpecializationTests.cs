using Core.Agents;

namespace Core.Tests.Agents
{
    public class AgentSpecializationTests
    {
        [Theory]
        [InlineData("Network")]
        [InlineData("Infrastructure")]
        [InlineData("Database")]
        public void FindByName_WithKnownName_ReturnsMatchingInstance(string name)
        {
            var result = AgentSpecialization.FindByName(name);

            Assert.NotNull(result);
            Assert.Equal(name, result!.Name);
        }

        [Fact]
        public void FindByName_WithUnknownName_ReturnsNull()
        {
            Assert.Null(AgentSpecialization.FindByName("Storage"));
        }

        [Fact]
        public void FindByName_ReturnsTheSameSharedInstanceAsTheStaticField()
        {
            Assert.Equal(AgentSpecialization.Network, AgentSpecialization.FindByName("Network"));
        }
    }
}
