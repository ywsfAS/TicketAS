using Core.Utilities;

namespace Core
{
    public sealed record ReporterId(Guid Id) : StrongTypedId(Id);
    public class Reporter : Entity<ReporterId>
    {

    }
}
