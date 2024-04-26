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
public class MatchEventQuery
{
    [UsedImplicitly]
    public Task<MatchEvent> GetMatchEvent(Guid id, MatchEventDataLoader loader) => loader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<MatchEvent> GetMatchEvents(TournamentContext dbContext)
    {
        return dbContext.MatchEvents;
    }
}

[UsedImplicitly]
public class MatchEventDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, MatchEvent>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, MatchEvent>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.MatchEvents
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(MatchPlayerEvent))]
public class MatchEventExtensions
{
    [UsedImplicitly]
    public Player GetPlayer(TournamentContext dbContext, [Parent] MatchPlayerEvent parent)
    {
        return dbContext.Players.Find(parent.PlayerId) ?? new Player();
    }
}