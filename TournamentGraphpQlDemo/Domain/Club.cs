namespace TournamentGraphpQlDemo.Domain;

public class Club
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    [IsProjected(false)]
    public List<Player> Players { get; set; } = new();

    [IsProjected(false)]
    public List<Team> Teams { get; set; } = new();

}