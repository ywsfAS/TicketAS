using System;
using System.Collections.Generic;

namespace Core.Reportes.ReportStates
{
    public sealed class BlockedReporterState : ReporterState
    {
        public override string Name { get; } = "Blocked";
        public override bool EnsureCanReport() => false;

        public override bool EnsureCanSendMessage() => false;

        public override bool CanSuspend() => false;

        public override bool CanActivate() => true;

        public override bool CanBlock() => false;
    }
}
