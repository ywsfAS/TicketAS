using Core.Exceptions;

namespace Core.Incidents
{

    public sealed record IncidentDescription
    {
        private const int MinLength = 10;
        private const int MaxLength = 500;

        public string Description { get; }

        private IncidentDescription(string description) => Description = description;
        public static IncidentDescription Create(string description)
        {
            description = description.Trim();
            if (!IsValidDescription(description)) throw new IncidentDescriptionException($"invalid incident description : {description}");

            return new IncidentDescription(description);
        }
        public static IncidentDescription Create(IEnumerable<string> segments)
        {
            var description = string.Join(" ", segments).Trim();
            if (!IsValidDescription(description)) throw new IncidentDescriptionException($"invalid incident description : {description}");

            return new IncidentDescription(description);
        }
        private static bool IsValidDescription(string Description) => !string.IsNullOrEmpty(Description)
            && Description.Length <= MaxLength
            && Description.Length >= MinLength;

    }
}
