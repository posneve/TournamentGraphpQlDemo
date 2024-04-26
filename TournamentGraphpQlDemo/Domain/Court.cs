namespace TournamentGraphpQlDemo.Domain;

public class Court
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    [IsProjected(false)] public List<Match> Matches { get; set; } = new();
}