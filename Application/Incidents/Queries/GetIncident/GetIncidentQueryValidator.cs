using FluentValidation;

namespace Application.Incidents.Queries.GetIncident;

public sealed class GetIncidentQueryValidator : AbstractValidator<GetIncidentQuery>
{
    public GetIncidentQueryValidator()
    {
        RuleFor(query => query.IncidentId).NotEmpty();
        RuleFor(query => query.UserId).NotEmpty();
    }
}
