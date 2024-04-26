using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentGraphpQlDemo.Domain;

namespace TournamentGraphpQlDemo.Infrastructure.EntityFramework;

public class TournamentContextConfiguration :
    IEntityTypeConfiguration<Player>,
    IEntityTypeConfiguration<Match>,
    IEntityTypeConfiguration<MatchEvent>,
    IEntityTypeConfiguration<MatchGenericEvent>,
    IEntityTypeConfiguration<MatchPlayerEvent>,
    IEntityTypeConfiguration<Court>,
    IEntityTypeConfiguration<Team>,
    IEntityTypeConfiguration<Club>
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
        
        builder.Property(x => x.Name).HasMaxLength(500);
    }

    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasOne(x => x.Club)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.ClubId);
        
        builder.Property(x => x.Name).HasMaxLength(500);
    }
    
    public void Configure(EntityTypeBuilder<MatchEvent> builder)
    {
        builder.HasOne(x => x.Match)
            .WithMany(x => x.MatchEvents)
            .HasForeignKey(x => x.MatchId);

        builder.HasDiscriminator<string>("EventType")
            .HasValue<MatchPlayerEvent>("MatchPlayerEvent")
            .HasValue<MatchGenericEvent>("MatchGenericEvent");
    }

    public void Configure(EntityTypeBuilder<MatchGenericEvent> builder)
    {
        builder.Property(x => x.EventType)
            .HasConversion<string>()
            .HasMaxLength(100);
    }
    
    public void Configure(EntityTypeBuilder<MatchPlayerEvent> builder)
    {
        builder.HasOne(x => x.Player)
            .WithMany(x => x.MatchEvents)
            .HasForeignKey(x => x.PlayerId);
        
        builder.Property(x => x.EventType)
            .HasConversion<string>()
            .HasMaxLength(100);
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

    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(500);
    }

    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(500);
    }
}