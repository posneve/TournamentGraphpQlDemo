using AutoMapper;
using TournamentGraphpQlDemo.Domain;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace TournamentGraphpQlDemo.GraphQL.DtoExample;

public class DtoMapperProfile : Profile
{
    public DtoMapperProfile()
    {
        CreateMap<Player, PlayerDto>()
            .ForMember(x=>x.NameAndId, o=>o.MapFrom(s=>$"{s.Name}-{s.Id}"));
        
        CreateMap<Team, TeamDto>();
    }

    public static IConfigurationProvider ConfigurationProvider =>
        new MapperConfiguration(cfg => cfg.AddProfile(new DtoMapperProfile()));
}