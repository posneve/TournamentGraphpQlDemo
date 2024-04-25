using AutoMapper.QueryableExtensions;
using GreenDonut;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Queries.DtoExample;

[ExtendObjectType(OperationType.Query)]
public class PlayerDtoQuery
{

        [UsedImplicitly]
        public Task<PlayerDto> GetPlayerDto(Guid id, PlayerDtoDataLoader playerDataLoader) => playerDataLoader.LoadAsync(id);

        [UsedImplicitly]
        [UseFiltering]
        public IQueryable<PlayerDto> GetPlayersDto(TournamentContext dbContext)
        {
            return dbContext.Players.ProjectTo<PlayerDto>(DtoMapperProfile.ConfigurationProvider);
        }
    
}

[UsedImplicitly]
public class PlayerDtoDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, PlayerDto>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, PlayerDto>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Players
            .Where(s => keys.Contains(s.Id))
            .ProjectTo<PlayerDto>(DtoMapperProfile.ConfigurationProvider)
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

[ExtendObjectType(typeof(PlayerDto))]
public class PlayerExtensions
{
    [UsedImplicitly]
    public IQueryable<TeamDto> GetTeams(TournamentContext dbContext, [Parent] PlayerDto parent)
    {
        return dbContext.Players.Include(player => player.Teams)
                   .Where(x => x.Id == parent.Id).SelectMany(x=>x.Teams).ProjectTo<TeamDto>(DtoMapperProfile.ConfigurationProvider);
    }
}


public class PlayerDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string NameAndId { get; set; }
}