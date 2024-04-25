using GreenDonut;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Queries;

[ExtendObjectType(OperationType.Query)]
public class MatchQuery
{
    [UsedImplicitly]
    public Task<Match> GetMatch(Guid id, MatchDataLoader playerDataLoader) => playerDataLoader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<Match> GetMatches(TournamentContext dbContext)
    {
        return dbContext.Matches;
    }
}

[UsedImplicitly]
public class MatchDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Match>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Match>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return await dbContext.Matches
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Match))]
public class MatchExtensions
{
    [UsedImplicitly]
    public Team GetHomeTeam(TournamentContext dbContext, [Parent] Match parent)
    {
        return dbContext.Teams.Find(parent.HomeTeamId) ?? new Team();
    }

    [UsedImplicitly]
    public Team GetGuestTeam(TournamentContext dbContext, [Parent] Match parent)
    {
        return dbContext.Teams.Find(parent.GuestTeamId) ?? new Team();
    }

    [UsedImplicitly]
    public Court GetCourt(TournamentContext dbContext, [Parent] Match parent)
    {
        return dbContext.Matches.Include(x => x.Court).FirstOrDefault(x => x.Id == parent.Id)?.Court ?? new Court();
    }

    [UsedImplicitly]
    public IQueryable<Goal> GetGoals(TournamentContext dbContext, [Parent] Match parent)
    {
        return dbContext.Matches.Include(x => x.Goals)
            .Where(x => x.Id == parent.Id)
            .SelectMany(x => x.Goals);
    }
}