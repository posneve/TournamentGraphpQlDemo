using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;

namespace TournamentGraphpQlDemo.Infrastructure.EntityFramework;

public class TournamentContext : DbContext
{
    public TournamentContext(DbContextOptions<TournamentContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<MatchEvent> MatchEvents { get; set; }
    public DbSet<Court> Courts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TournamentContext).Assembly);
    }
}