namespace TournamentGraphpQlDemo.Domain;

public class Goal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid Match { get; set; }
    [IsProjected(false)]
    public Guid PlayerId { get; set; }
    [IsProjected(false)]
    public Player Player { get; set; }
    public DateTime Time { get; set; }
}