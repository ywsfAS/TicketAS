
namespace Core.Exceptions
{
    public sealed class ReporterIsNullException() : DomainException("Reporter cannot be null");
    public sealed class ReporterInvalidStateTransitionException(string from , string to) : DomainException($"Reporter invalid transition state from {from} => {to} ");
    public sealed class ReporterInvalidActionForStateException() : DomainException("Reporter cannor apply this action in the current state");
}
