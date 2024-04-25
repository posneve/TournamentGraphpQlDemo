using GreenDonut;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL;


[ExtendObjectType(OperationType.Query)]
public class TeamQuery
{
    [UsedImplicitly]
    public Task<Team> GetTeam(Guid id, TeamDataLoader playerDataLoader) => playerDataLoader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<Team> GetTeams(TournamentContext dbContext)
    {
        return dbContext.Teams;
    }
}

[UsedImplicitly]
public class TeamDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Team>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Team>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Teams
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Team))]
public class TeamExtensions
{
    [UsedImplicitly]
    public Club GetClub(TournamentContext dbContext, [Parent] Team parent)
    {
        return dbContext.Teams.Include(x => x.Club)
            .First(x => x.Id == parent.Id).Club ?? new Club();
    }
    
    [UsedImplicitly]
    public IQueryable<Match> GetMatches(TournamentContext dbContext, [Parent] Team parent)
    {
        return dbContext.Teams
            .Include(x => x.GuestMatches)
            .Include(x => x.HomeMatches)
            .Where(x => x.Id == parent.Id)
            .SelectMany(x=>x.GuestMatches.Concat(x.HomeMatches));
    }
}