
namespace Core.Exceptions
{
    public sealed class AgentStateIsNullException() : DomainException("Agent state cannot be null");
    public sealed class AgentInvalidStateTransitionException(string from , string to) : DomainException($"Agent state transition is invalid from {from} => {to}");
}
