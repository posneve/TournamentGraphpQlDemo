namespace TournamentGraphpQlDemo.Domain;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public List<Player> Players { get; set; } = new();
    [IsProjected(false)] public List<Match> HomeMatches { get; set; } = new();
    [IsProjected(false)] public List<Match> GuestMatches { get; set; } = new();


    [IsProjected(false)]
    public Club Club { get; set; } = new();

    public Guid ClubId { get; set; }
}