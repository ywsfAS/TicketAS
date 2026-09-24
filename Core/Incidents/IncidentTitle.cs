
using Core.Exceptions;

namespace Core.Incidents
{
    public sealed record IncidentTitle
    {
        private const int MaxProblemLength = 200;
        private const int MaxServiceLength = 100;

        private static char[] InvalidSegments = ['-', '/'];
        private const char del = '-';
        private static string[] InvalidProblemKeywords = ["service"];
        private static string[] InvalidServiceKeywords = ["problem"];

        public string Problem { get; }
        public string AffectedService { get; }

        public string Title => $"[{Problem}] {del} [{AffectedService}]";


        private IncidentTitle(string problem, string service) => (Problem, AffectedService) = (problem, service);
        public static IncidentTitle Create(string problem, string service)
        {
            problem = problem.Trim();
            service = service.Trim();
            if (!IsValidProblem(problem)) throw new IncidentTitleProblemIsInvalidException(problem);
            if (!IsValidService(service)) throw new IncidentTitleServiceIsInvalidException(service);

            return new IncidentTitle(problem, service);
        }
        public static IncidentTitle Create(string title)
        {
            var (problem, service) = SplitTitle(title);

            problem = problem.Trim();
            service = service.Trim();

            if (!IsValidProblem(problem)) throw new IncidentTitleProblemIsInvalidException(problem);
            if (!IsValidService(service)) throw new IncidentTitleServiceIsInvalidException(service);

            return new IncidentTitle(problem, service);

        }
        private static (string, string) SplitTitle(string title)
        {
            var segments = title.Split(del, 2);
            if (segments.Length < 2) throw new IncidentTitleException("Invalid incident title format");
            return (segments[0], segments[1]);
        }
        private static bool IsValidProblem(string problem) => !string.IsNullOrEmpty(problem) && problem.Length <= MaxProblemLength && !problem.Any((c) => InvalidSegments.Contains(c)) && !problem.Split(" ").Any((w) => InvalidProblemKeywords.Contains(w));
        private static bool IsValidService(string service) => !string.IsNullOrEmpty(service) && service.Length <= MaxServiceLength && !service.Any((c) => InvalidSegments.Contains(c)) && !service.Split(" ").Any((w) => InvalidServiceKeywords.Contains(w));

    }
}
