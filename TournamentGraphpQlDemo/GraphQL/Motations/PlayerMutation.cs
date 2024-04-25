using HotChocolate.Language;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.GraphQL.Motations.Exceptions;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Motations;

[ExtendObjectType(OperationType.Mutation)]
public class PlayerMutation
{
    
    [Error(typeof(ClubDoesNotExistException))]
    public async Task<Player> PlayerAdd(PlayerAddInput input, TournamentContext ctx, CancellationToken ct)
    {
        var club = await ctx.Clubs.FirstOrDefaultAsync(x => x.Id == input.ClubId, ct);
        if (club == null) throw new ClubDoesNotExistException(input.ClubId);

        var player = new Player
        {
            Name = input.Name,
            Clubs = new List<Club>
            {
                club
            }
        };
        ctx.Add(player);
        await ctx.SaveChangesAsync(ct);
        return player;
    }
}

public record PlayerAddInput
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