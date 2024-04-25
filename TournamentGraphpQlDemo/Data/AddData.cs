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


        var goals = new List<Goal>
        {
            new()
            {
                Player = player,
                Time = DateTime.Now
            },
            new()
            {
                Player = player,
                Time = DateTime.Now.AddMinutes(5)
            }
        };
        
        var match = new Match
        {
            Time = DateTime.Today.AddDays(2),
            HomeTeam = team,
            GuestTeam = team2,
            Court = court, 
            Goals = goals,
        };
        
        var match2 = new Match
        {
            Time = DateTime.Today.AddDays(4),
            HomeTeam = team2,
            GuestTeam = team,
            Court = court2
        };

        var match3 = new Match
        {
            Time = DateTime.Today.AddDays(2),
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