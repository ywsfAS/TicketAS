using Core.Exceptions;
using Core.Incidents;

namespace Core.Tests.Incidents
{
    public class IncidentDescriptionTests
    {
        [Fact]
        public void Create_WithValidDescription_Succeeds()
        {
            var description = IncidentDescription.Create("Users cannot reach the payments API.");

            Assert.Equal("Users cannot reach the payments API.", description.Description);
        }

        [Fact]
        public void Create_TrimsWhitespace()
        {
            var description = IncidentDescription.Create("   Users cannot reach the payments API.   ");

            Assert.Equal("Users cannot reach the payments API.", description.Description);
        }

        [Fact]
        public void Create_WithNullString_Throws()
        {
            Assert.Throws<IncidentDescriptionIsNullException>(() => IncidentDescription.Create((string)null!));
        }

        [Fact]
        public void Create_WithNullSegments_Throws()
        {
            Assert.Throws<IncidentDescriptionIsNullException>(() => IncidentDescription.Create((IEnumerable<string>)null!));
        }

        [Fact]
        public void Create_ShorterThanMinLength_Throws()
        {
            Assert.Throws<IncidentDescriptionException>(() => IncidentDescription.Create("short"));
        }

        [Fact]
        public void Create_Empty_Throws()
        {
            Assert.Throws<IncidentDescriptionException>(() => IncidentDescription.Create(""));
        }

        [Fact]
        public void Create_LongerThanMaxLength_Throws()
        {
            var tooLong = new string('a', 501);

            Assert.Throws<IncidentDescriptionException>(() => IncidentDescription.Create(tooLong));
        }

        [Fact]
        public void Create_AtExactlyMaxLength_Succeeds()
        {
            var exactlyMax = new string('a', 500);

            var description = IncidentDescription.Create(exactlyMax);

            Assert.Equal(500, description.Description.Length);
        }

        [Fact]
        public void Create_FromSegments_JoinsWithSpaces()
        {
            var description = IncidentDescription.Create(new[] { "Users", "cannot", "reach", "the", "payments", "API." });

            Assert.Equal("Users cannot reach the payments API.", description.Description);
        }
    }
}
