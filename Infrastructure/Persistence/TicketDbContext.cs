using Core;
using Core.Agents;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Reporters;
using Core.Tickets.Conversation;
using Core.Users;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public sealed class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<Specialization> Specializations => Set<Specialization>();
        public DbSet<IncidentCategory> IncidentCategories => Set<IncidentCategory>();
        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<Reporter> Reporters => Set<Reporter>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Conversation> Conversations => Set<Conversation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketDbContext).Assembly);

            foreach (var type in modelBuilder.Model.GetEntityTypes().Select(t => t.ClrType).ToList())
            {
                if (IsDomainEntity(type))
                    modelBuilder.Entity(type).Ignore(nameof(Entity<UserId>.Events));
            }
        }

        private static bool IsDomainEntity(Type type)
        {
            for (var t = type.BaseType; t is not null; t = t.BaseType)
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Entity<>)) return true;
            return false;
        }
    }
}

