using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.GraphQL.Mutations.Exceptions;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Mutations;

[ExtendObjectType(OperationType.Mutation)]
public class PlayerMutation
{
    [UsedImplicitly]
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
    
    [UsedImplicitly]
    [Error(typeof(ClubDoesNotExistException))]
    public async Task<Player> PlayerAddToTeam(PlayerAddToTeamInput input, TournamentContext ctx, CancellationToken ct)
    {
        var player = await ctx.Players.Include(x=>x.Teams).FirstOrDefaultAsync(x => x.Id == input.PlayerId, ct);
        if (player == null) throw new PlayerDoesNotExistException(input.PlayerId);
        if (player.Teams.Any(x => x.Id == input.TeamId)) return player;
        
        var team = await ctx.Teams.FirstOrDefaultAsync(x => x.Id == input.TeamId, ct);
        if (team == null) throw new TeamDoesNotExistException(input.PlayerId);
        player.Teams.Add(team);
        await ctx.SaveChangesAsync(ct);
        return player;
    }
}

public record PlayerAddToTeamInput
{
    public Guid PlayerId { get; set; }
    public Guid TeamId { get; set; }
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