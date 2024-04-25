using Microsoft.EntityFrameworkCore;
using TournamentGraphpQlDemo;
using TournamentGraphpQlDemo.Data;
using TournamentGraphpQlDemo.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

var inmemoryDbContextName = Guid.NewGuid().ToString();
builder.Services.AddPooledDbContextFactory<TournamentContext>(o => 
    o.UseInMemoryDatabase(inmemoryDbContextName));


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

await AddData.AddSomeData(builder.Services);


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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

namespace TournamentGraphpQlDemo
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}