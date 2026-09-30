using Microsoft.EntityFrameworkCore;
using Core.Users;
using Core.Agents;
using Core.Incidents;
using Core.Reporters;
using Core;

namespace Infrastructure.Persistence
{
    public sealed class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // Load configurations from Configuration/ 
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketDbContext).Assembly);

    }
}
