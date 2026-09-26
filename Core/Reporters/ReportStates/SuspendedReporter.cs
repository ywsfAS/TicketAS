namespace Core.Reporters.ReportStates
{
    public sealed class SuspendedReporterState : ReporterState
    {
        public override string Name { get; } = "Suspended";
        public override bool EnsureCanReport() => false;

        public override bool EnsureCanSendMessage() => false;

        public override bool CanSuspend() => false;

        public override bool CanActivate() => true;

        public override bool CanBlock() => false;
    }
}
