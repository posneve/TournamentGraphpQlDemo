using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Court> Courts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TournamentContext).Assembly);
    }
}

public class TournamentContextConfituration :
    IEntityTypeConfiguration<Player>,
    IEntityTypeConfiguration<Match>,
    IEntityTypeConfiguration<Goal>,
    IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Teams)
            .WithMany(x => x.Players);

        builder
            .HasMany(x => x.Clubs)
            .WithMany(x => x.Players);
        
    }

    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasOne(x => x.Club)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.ClubId);
    }
    
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasOne(x => x.Player)
            .WithMany(x => x.Goals)
            .HasForeignKey(x => x.PlayerId);
    }

    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasOne(x => x.HomeTeam)
            .WithMany(x => x.HomeMatches)
            .HasForeignKey(x => x.HomeTeamId);

        builder.HasOne(x => x.GuestTeam)
            .WithMany(x => x.GuestMatches)
            .HasForeignKey(x => x.GuestTeamId);

        builder.HasOne(x => x.Court)
            .WithMany(x => x.Matches)
            .HasForeignKey(x => x.CourtId);
    }
}