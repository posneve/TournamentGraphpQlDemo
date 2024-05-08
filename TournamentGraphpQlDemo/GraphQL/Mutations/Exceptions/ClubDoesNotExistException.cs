namespace TournamentGraphpQlDemo.GraphQL.Mutations.Exceptions;

public class ClubDoesNotExistException(Guid clubId) : Exception($"Club id {clubId} does not exist")
{
    public Guid ClubId { get; set; } = clubId;
}

public class PlayerDoesNotExistException(Guid playerId) : Exception($"Player id {playerId} does not exist")
{
    public Guid PlayerId { get; set; } = playerId;
}

public class TeamDoesNotExistException(Guid teamId) : Exception($"Team id {teamId} does not exist")
{
    public Guid TeamId { get; set; } = teamId;
}