namespace TournamentGraphpQlDemo.Domain;

public class Match
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [IsProjected(false)] 
    public Guid CourtId { get; set; }

    [IsProjected(false)] public Court Court { get; set; } = new();
    [IsProjected(false)] public Guid HomeTeamId { get; set; }
    [IsProjected(false)] public Team HomeTeam { get; set; } = null!;

    [IsProjected(false)] public Guid GuestTeamId { get; set; }
    [IsProjected(false)] public Team GuestTeam { get; set; } = null!;

    public DateTime Time { get; set; }
    [IsProjected(false)]
    public List<Goal> Goals { get; set; } = new();
}