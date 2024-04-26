namespace TournamentGraphpQlDemo.Domain;

public class Match
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [IsProjected(false)] public Guid CourtId { get; set; }

    [IsProjected(false)] public Court Court { get; set; } = new();
    [IsProjected(false)] public Guid HomeTeamId { get; set; }
    [IsProjected(false)] public Team HomeTeam { get; set; } = null!;

    [IsProjected(false)] public Guid GuestTeamId { get; set; }
    [IsProjected(false)] public Team GuestTeam { get; set; } = null!;

    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    [IsProjected(false)] public List<MatchEvent> MatchEvents { get; set; } = new();

    public void AddPlayerEvent(Player player, MatchPlayerEventType eventType, Team team)
    {
        MatchEvents.Add(
            new MatchPlayerEvent
            {
                Player = player,
                EventType = eventType,
                IsHomeTeam = team == HomeTeam,
                Time = TimeOnly.FromDateTime(DateTime.Now)
            });
    }

    public void AddGenericEvent(MatchGenericEventType eventType)
    {
        MatchEvents.Add(
            new MatchGenericEvent
            {
                EventType = eventType,
                Time = TimeOnly.FromDateTime(DateTime.Now)
            });
    }
}