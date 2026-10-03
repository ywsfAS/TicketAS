using Core.Incidents.Categories;
using Core.Utilities;

namespace Core.Agents
{
    public sealed record IncidentSpecializationId(Guid Id) : StrongTypedId(Id);
    public sealed class IncidentCategorySpecialization : Entity<IncidentSpecializationId>
    {
        public IncidentCategoryId CategoryId { get; }

        public SpecializationId SpecializationId { get; }

        private IncidentCategorySpecialization() { }

        public IncidentCategorySpecialization(
            IncidentCategoryId categoryId,
            SpecializationId specializationId)
        {
            Id = new IncidentSpecializationId(Guid.NewGuid());
            CategoryId = categoryId;
            SpecializationId = specializationId;
        }
    }
}
