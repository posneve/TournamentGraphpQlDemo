using AutoMapper.QueryableExtensions;
using GreenDonut;
using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.GraphQL.Queries.DtoExample;

[ExtendObjectType(OperationType.Query)]
public class TeamDtoQuery
{

        [UsedImplicitly]
        public Task<TeamDto> GetTeamDto(Guid id, TeamDtoDataLoader dataLoader) => dataLoader.LoadAsync(id);

        [UsedImplicitly]
        [UseFiltering]
        public IQueryable<TeamDto> GetTeamsDto(TournamentContext dbContext)
        {
            return dbContext.Teams.ProjectTo<TeamDto>(DtoMapperProfile.ConfigurationProvider);
        }
    
}

[UsedImplicitly]
public class TeamDtoDataLoader(
    IDbContextFactory<TournamentContext> dbContextFactory,
    IBatchScheduler batchScheduler,
    DataLoaderOptions options)
    : BatchDataLoader<Guid, TeamDto>(batchScheduler, options)
{
    protected override async Task<IReadOnlyDictionary<Guid, TeamDto>>
        LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken ct)
    {
      await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return  await dbContext.Teams
            .Where(s => keys.Contains(s.Id))
            .ProjectTo<TeamDto>(DtoMapperProfile.ConfigurationProvider)
            .ToDictionaryAsync(t => t.Id, ct);
    }
}

public class TeamDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
}