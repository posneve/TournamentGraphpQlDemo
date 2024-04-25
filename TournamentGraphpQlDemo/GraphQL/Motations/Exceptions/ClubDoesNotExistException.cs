namespace TournamentGraphpQlDemo.GraphQL.Motations.Exceptions;

public class ClubDoesNotExistException(Guid clubId) : Exception
{
    public Guid ClubId { get; set; } = clubId;
}