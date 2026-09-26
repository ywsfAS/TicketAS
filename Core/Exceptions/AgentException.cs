
namespace Core.Exceptions
{
    public sealed class AgentIsNullException() : DomainException("Agent cannot be null");
    public sealed class AgentSeniorityIsNullException() : DomainException("Agent's seniority cannot be null");
    public sealed class AgentStateIsNullException() : DomainException("Agent state cannot be null");
    public sealed class AgentInvalidStateTransitionException(string from , string to) : DomainException($"Agent state transition is invalid from {from} => {to}");
    public sealed class AgentInvalidActionForStateException() : DomainException("Agent cannot apply this action in current state");
}
