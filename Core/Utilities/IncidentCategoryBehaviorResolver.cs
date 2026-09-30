using Core.Incidents.Categories;

namespace Core.Utilities
{
    public interface IIncidentCategoryBehaviorResolver
    {
        IIncidentCategoryBehavior Resolve(IncidentCategory category);
    }
    public sealed class IncidentCategoryBehaviorResolver
    : IIncidentCategoryBehaviorResolver
    {
        public IIncidentCategoryBehavior Resolve(IncidentCategory category)
        {
            return category.Name switch
            {
                "Network" => ResolveNetwork(category),

                _ => throw new InvalidOperationException(
                    $"No behavior registered for category '{category.Name}'.")
            };
        }

        private static NetworkIncident ResolveNetwork(
            IncidentCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
