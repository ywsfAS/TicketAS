namespace Core.Agents
{
    public sealed record AgentSpecialization 
    {
        public static readonly AgentSpecialization Network = new("Network");
        public static readonly AgentSpecialization Infrastructure = new("Infrastructure");
        public static readonly AgentSpecialization Database = new("Database");

        private static readonly AgentSpecialization[] All = [Network,Infrastructure,Database];  

        public string Name { get; private set; }

        private AgentSpecialization(string name) => Name = name;

        public static AgentSpecialization? FindByName(string name) =>
            All.FirstOrDefault((n) => n.Name == name);

    }
}
