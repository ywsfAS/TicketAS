
namespace Core.Reportes.ReportStates
{
    public sealed class ActiveReporterState : ReporterState
    {
        public override string Name { get; } = "Active";
        public override bool EnsureCanReport() => true;
        public override bool EnsureCanSendMessage() => true;


        public override bool CanSuspend() => true;
        public override bool CanActivate() => true;
        public override bool CanBlock() => true;
    }
}
