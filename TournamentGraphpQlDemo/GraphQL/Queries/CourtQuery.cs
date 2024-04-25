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
public class CourtQuery
{
    [UsedImplicitly]
    public Task<Court> GetCourt(Guid id, CourtDataLoader loader) => loader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<Court> GetCourts(TournamentContext dbContext)
    {
        return dbContext.Courts;
    }
}

[UsedImplicitly]
public class CourtDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Court>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Court>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Courts
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Court))]
public class CourtExtensions
{
    [UsedImplicitly]
    public IQueryable<Match> GetMatches(TournamentContext dbContext, [Parent] Court parent)
    {
        return dbContext.Courts.Include(x=>x.Matches)
            .Where(x=>x.Id == parent.Id)
            .SelectMany(x=>x.Matches);
    }
}