using Core.Incidents;
using FluentValidation;

namespace Application.Incidents.Commands.ChangeIncidentTitle;

public sealed class ChangeIncidentTitleCommandValidator : AbstractValidator<ChangeIncidentTitleCommand>
{
    public ChangeIncidentTitleCommandValidator()
    {
        RuleFor(command => command.IncidentId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Text)
            .Must(IsValidTitle)
            .WithMessage("Title must contain a valid problem and service separated by a hyphen.");
    }

    private static bool IsValidTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return false;

        var parts = title.Split('-', 2);
        return parts.Length == 2
            && IncidentTitle.IsValidProblem(parts[0].Trim())
            && IncidentTitle.IsValidService(parts[1].Trim());
    }
}
