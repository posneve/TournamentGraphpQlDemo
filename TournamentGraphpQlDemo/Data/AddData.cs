using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

namespace TournamentGraphpQlDemo.Data;

public class AddData
{
    public static async Task AddSomeData(IServiceCollection sc)
    {
        await using var sp = sc.BuildServiceProvider();
        var dbContextFactory = sp.GetRequiredService<IDbContextFactory<TournamentContext>>();
        await using var dbContext =
            await dbContextFactory.CreateDbContextAsync();
        
        await AddSomeData(dbContext);
    }

    private static async Task AddSomeData(TournamentContext context)
    {
        var club = new Club
        {
            Name = "Play Club"
        };
        
        var players = new List<Player>
        {
            new Player()
            {
                Name = "Hans Play",
                Clubs = new List<Club>
                {
                    club
                }
                    
            },
            new Player()
            {
                Name = "Jan Flay",
            },
            new Player()
            {
                Name = "Jon Do",
            },
        };
        
        var team = new Team
        {
            Name = "Play Team",
            Club = club,
            Players = players
        };

        var player = new Player()
        {
            Name = "Kris Fly",
        };
        
        var players2 = new List<Player>
        {
            player,
            new Player()
            {
                Name = "Kong Donk",
            },
            new Player()
            {
                Name = "Donald Trumph",
            },
        };
        
        var club2 = new Club
        {
            Name = "Cheat Club",
            Players = players2
        };

        
        var team2 = new Team
        {
            Name = "Cheat Team",
            Club = club2,
            Players = players2
        };

        var court = new Court
        {
            Name = "Powerhouse Court"
        };
        var court2 = new Court
        {
            Name = "Bakklandet Church Court"
        };


        var matchEvent = new List<MatchEvent>
        {
            new MatchGenericEvent
            {
                EventType = MatchGenericEventType.MatchStarted,
                Time = TimeOnly.FromDateTime(DateTime.Now)
            },
            new MatchPlayerEvent()
            {
                Player = player,
                Time = TimeOnly.FromDateTime(DateTime.Now),
                EventType = MatchPlayerEventType.Goal
            },
            new MatchPlayerEvent()
            {
                Player = player,
                Time = TimeOnly.FromDateTime(DateTime.Now).AddHours(12).AddMinutes(5),
                EventType = MatchPlayerEventType.Goal
            },
            new MatchGenericEvent
            {
            EventType = MatchGenericEventType.FirstPeriodFinished,
            Time = TimeOnly.FromDateTime(DateTime.Now).AddHours(12).AddMinutes(20)
        },
            new MatchGenericEvent
            {
                EventType = MatchGenericEventType.SecondPeriodeStared,
                Time = TimeOnly.FromTimeSpan(new TimeSpan(0,0,25))
            },
            
            new MatchPlayerEvent()
            {
                Player = player,
                Time = TimeOnly.FromDateTime(DateTime.Now),
                EventType = MatchPlayerEventType.Goal
            },
            new MatchPlayerEvent()
            {
                Player = player,
                Time = TimeOnly.FromDateTime(DateTime.Now).AddMinutes(5),
                EventType = MatchPlayerEventType.Goal
            },
            new MatchGenericEvent
            {
                EventType = MatchGenericEventType.MatchFinished,
                Time = TimeOnly.FromDateTime(DateTime.Now).AddHours(12).AddMinutes(50)
            },
        };
        
        var match = new Match
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            Time = new TimeOnly(12,0),
            HomeTeam = team,
            GuestTeam = team2,
            Court = court, 
            MatchEvents = matchEvent,
        };
        
        var match2 = new Match
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            Time = new TimeOnly(12,0),
            HomeTeam = team2,
            GuestTeam = team,
            Court = court2
        };

        var match3 = new Match
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            Time = new TimeOnly(12,0),
            HomeTeam = team2,
            GuestTeam = team,
            Court = court2
        };

        context.Add(team);
        context.Add(team2);
        context.Add(club);
        context.Add(club2);
        context.Add(match);
        context.Add(match2);
        context.Add(match3);
        await context.SaveChangesAsync();
    }
}