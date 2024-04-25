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
public class GoalQuery
{
    [UsedImplicitly]
    public Task<Goal> GetGoal(Guid id, GoalDataLoader loader) => loader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<Goal> GetGoals(TournamentContext dbContext)
    {
        return dbContext.Goals;
    }
}

[UsedImplicitly]
public class GoalDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Goal>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Goal>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Goals
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Goal))]
public class GoalExtensions
{
    [UsedImplicitly]
    public Player GetPlayer(TournamentContext dbContext, [Parent] Goal parent)
    {
        return dbContext.Players.Find(parent.PlayerId) ?? new Player();
    }
}