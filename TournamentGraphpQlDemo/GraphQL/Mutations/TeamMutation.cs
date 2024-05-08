using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.GraphQL.Mutations.Exceptions;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Mutations;

[ExtendObjectType(OperationType.Mutation)]
public class TeamMutation
{
    [Error(typeof(ClubDoesNotExistException))]
    [UsedImplicitly]
    public async Task<Team> TeamAdd(TeamAddInput input, TournamentContext ctx, CancellationToken ct)
    {
        var club = await ctx.Clubs.FirstOrDefaultAsync(x => x.Id == input.ClubId, ct);
        if (club == null) throw new ClubDoesNotExistException(input.ClubId);

        var team = new Team()
        {
            Name = input.Name,
            Club = club
        };
        ctx.Add(team);
        await ctx.SaveChangesAsync(ct);
        return team;
    }
}

public record TeamAddInput
{
    /// <summary>
    /// Name of player
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Id of club the player is a member of
    /// A player need to be a member of at least one club
    /// </summary>
    public Guid ClubId { get; set; }
}