using Core.Utilities;

namespace Core.Agents
{
    public sealed record SpecializationId(Guid id) : StrongTypedId(id);
    public sealed class Specialization : Entity<SpecializationId>
    {
        private static readonly Specialization Network = new("Network");
        private static readonly Specialization Infrastructure = new("Infrastructure");
        private static readonly Specialization Database = new("Database");

        private static readonly Specialization[] All = [Network, Infrastructure, Database];


        public string Name { get; private set; }

        private Specialization(string name) => Name = name;

        public override string ToString() => $"Specialization : {Name}";
        public static Specialization? findByName(string name) => All.FirstOrDefault(s => s.Name == name);

    }
}
