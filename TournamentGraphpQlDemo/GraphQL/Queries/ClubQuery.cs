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
public class ClubQuery
{
    [UsedImplicitly]
    public Task<Club> GetClub(Guid id, ClubDataLoader loader) => loader.LoadAsync(id);

    [UsedImplicitly]
    public IQueryable<Club> GetClubs(TournamentContext dbContext)
    {
        return dbContext.Clubs;
    }
}

[UsedImplicitly]
public class ClubDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, Club>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, Club>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Clubs
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(Club))]
public class ClubExtensions
{
    [UsedImplicitly]
    public IQueryable<Player> GetPlayers(TournamentContext dbContext, [Parent] Club parent)
    {
        return dbContext.Clubs.Include(club => club.Players)
                   .Where(x => x.Id == parent.Id)
                   .SelectMany(x=>x.Players);
    }   
    
    [UsedImplicitly]
    public IQueryable<Team> GetTeams(TournamentContext dbContext, [Parent] Club parent)
    {
        return dbContext.Clubs.Include(x => x.Teams)
                   .Where(x => x.Id == parent.Id)
                   .SelectMany(x=>x.Teams);
    }
}