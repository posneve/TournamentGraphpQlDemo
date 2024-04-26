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
public class PlayerQuery
{
    [UsedImplicitly]
    public Task<Player> GetPlayer(Guid id, PlayerDataLoader playerDataLoader) => playerDataLoader.LoadAsync(id);

    [UsedImplicitly]
    [UseFiltering]
    public IQueryable<Player> GetPlayers(TournamentContext dbContext)
    {
        return dbContext.Players;
    }
}

[UsedImplicitly]
public class PlayerDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Player>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Player>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Players
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Player))]
public class PlayerExtensions
{
    [UsedImplicitly]
    public IQueryable<Club> GetClubs(TournamentContext dbContext, [Parent] Player parent)
    {
        return dbContext.Players.Include(x => x.Clubs)
                   .Where(x => x.Id == parent.Id)
                   .SelectMany(x=>x.Clubs);
    }   
    [UsedImplicitly]
    public IQueryable<MatchEvent> GetGoals(TournamentContext dbContext, [Parent] Player parent)
    {
        return dbContext.Players.Include(x => x.MatchEvents)
            .Where(x => x.Id == parent.Id)
            .SelectMany(x=>x.MatchEvents);
    }  
    
    [UsedImplicitly]
    public IQueryable<Team> GetTeams(TournamentContext dbContext, [Parent] Player parent)
    {
        return dbContext.Players.Include(player => player.Teams)
                   .FirstOrDefault(x => x.Id == parent.Id)?.Teams.AsQueryable()
               ?? new List<Team>().AsQueryable();
    }
}