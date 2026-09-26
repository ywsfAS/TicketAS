namespace Core.Reportes.ReportStates
{
    public abstract class ReporterState
    {
        public abstract string Name { get; }
        public abstract bool EnsureCanReport();

        public abstract bool EnsureCanSendMessage();

        public abstract bool CanSuspend();

        public abstract bool CanActivate();

        public abstract bool CanBlock();
    }
}
