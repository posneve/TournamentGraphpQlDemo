using HotChocolate.Language;
using HotChocolate.Types;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Motations;

[ExtendObjectType(OperationType.Mutation)]
public class ClubMutation
{
    public async Task<Club> ClubAdd(PlayerAddInput input, TournamentContext ctx, CancellationToken ct)
    {
        var club = new Club
        {
            Name = input.Name
        };
        ctx.Add(club);
        await ctx.SaveChangesAsync(ct);
        return club;
    }
}


public record ClubAddInput
{
    /// <summary>
    /// Name of Club
    /// </summary>
    public string Name { get; set; } 
}