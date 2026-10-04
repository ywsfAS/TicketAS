namespace Api.Contracts;

public sealed record ReportIncidentRequest(
    string Problem,
    string Service,
    string Description,
    Guid CategoryId,
    string Scope,
    string Environment);
