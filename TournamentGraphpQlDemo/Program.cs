using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo.Domain;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddPooledDbContextFactory<TournamentContext>(o => 
        o.UseNpgsql(configuration.GetConnectionString("TournamentPostgresDb")
                )
        
    );


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddGraphQLServer()
    .AddTournamentTypes()
    .AddQueryType()
    .AddMutationType()
    .AddMutationConventions()
    .AddFiltering()
    .RegisterDbContext<TournamentContext>(DbContextKind.Pooled)
    .ModifyRequestOptions(o=>o.IncludeExceptionDetails = true)
    .AddProjections()
    .AddType<MatchPlayerEvent>()
    .AddType<MatchGenericEvent>()
    .AddInstrumentation() // if you want to use telemetry
    // .AddBananaCakePopServices(x =>
    // {
    //     x.ApiId = "VGhpcyBpcyBub3QgYSByZWFsIGFwaSBpZA==";
    //     x.ApiKey = "Tm9wZSwgdGhpcyBpcyBhbHNvIG5vIHJlYWwga2V5IDspIA==";
    //     x.Stage = "dev";
    // })
    //   .UseOnlyPersistedQueriesAllowed() // optional
    //.UsePersistedQueryPipeline() // if you want to use persisted queries
    ;

//await AddData.AddSomeData(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGraphQL();
app.MapBananaCakePop();

app.Run();

namespace TournamentGraphpQlDemo
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}