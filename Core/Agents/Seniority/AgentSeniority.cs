
namespace Core.Agents.Seniority
{
    public abstract record AgentSeniority : IComparable<AgentSeniority>
    {
        public static AgentSeniority Junior = new JuniorSeniority();
        public static AgentSeniority Mid = new MidSeniority();
        public static AgentSeniority Senior = new SeniorSeniority();

        private static readonly AgentSeniority[] All = [Junior, Mid , Senior];

        public string Name {  get; private set; }
        private readonly int _rank;

        protected AgentSeniority(string name, int rank) =>
            (Name,_rank) = (name,rank);
        public bool Meets(AgentSeniority other) => _rank >= other._rank;
        public int CompareTo(AgentSeniority other) => other is null ? 1 : _rank.CompareTo(other._rank); 

        public static AgentSeniority? FindByName(string name) => All.FirstOrDefault(x => x.Name == name);
    }
}
