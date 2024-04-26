namespace TournamentGraphpQlDemo.Domain;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    [IsProjected(false)] 
    public List<Team> Teams { get; set; } = new();

    [IsProjected(false)] 
    public List<Club> Clubs { get; set; } = new();
    [IsProjected(false)] 
    public List<MatchPlayerEvent> MatchEvents { get; set; } = new();
}