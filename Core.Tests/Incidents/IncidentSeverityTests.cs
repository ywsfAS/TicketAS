using Core.Incidents.Severity;

namespace Core.Tests.Incidents
{
    public class IncidentSeverityTests
    {
        [Fact]
        public void Low_HasLevel1() => Assert.Equal(1, new LowIncidentSeverity().Level);

        [Fact]
        public void Medium_HasLevel2() => Assert.Equal(2, new MeduimIncidentSeverity().Level);

        [Fact]
        public void High_HasLevel3() => Assert.Equal(3, new HighIncidentSeverity().Level);

        [Fact]
        public void Critical_HasLevel4() => Assert.Equal(4, new CriticalIncidentSeverity().Level);
    }

    public class IncidentSeverityExtensionTests
    {
        [Fact]
        public void Max_ReturnsTheHighestSeverityInTheList()
        {
            var list = new IncidentSeverity[]
            {
                new LowIncidentSeverity(),
                new CriticalIncidentSeverity(),
                new MeduimIncidentSeverity()
            };

            var result = IncidentSeverityExtension.Max(list);

            Assert.IsType<CriticalIncidentSeverity>(result);
        }

        [Fact]
        public void Max_WithASingleElement_ReturnsThatSameLevel()
        {
            var list = new IncidentSeverity[] { new HighIncidentSeverity() };

            var result = IncidentSeverityExtension.Max(list);

            Assert.IsType<HighIncidentSeverity>(result);
        }

        [Fact]
        public void Max_WithAllEqualLevels_ReturnsThatLevel()
        {
            var list = new IncidentSeverity[] { new LowIncidentSeverity(), new LowIncidentSeverity() };

            var result = IncidentSeverityExtension.Max(list);

            Assert.IsType<LowIncidentSeverity>(result);
        }

        [Fact]
        public void Max_WithEmptyList_Throws() =>
            Assert.Throws<InvalidOperationException>(() => IncidentSeverityExtension.Max(Array.Empty<IncidentSeverity>()));
    }
}
