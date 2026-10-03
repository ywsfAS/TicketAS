
using Core.Utilities;

namespace Core.Agents
{
    public sealed record AgentSpecializationId(Guid Id) : StrongTypedId(Id);
    public sealed class AgentSpecialization : Entity<AgentSpecializationId>
    {
        public AgentId AgentId { get; private set; }
        public SpecializationId SpecializationId { get; private set; }

        private AgentSpecialization(AgentId id, SpecializationId specialization) => (AgentId,SpecializationId) = (id , specialization);

        public static AgentSpecialization Create(AgentId id, SpecializationId specialization) => new AgentSpecialization(id, specialization);

    }
}
