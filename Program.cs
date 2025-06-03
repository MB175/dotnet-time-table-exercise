using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.Data;
using System.Globalization;

const int proxyPort = 5224;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwagger("/proxy/" + proxyPort);

var app = builder.Build();
app.UseSwaggerWithUI("/proxy/" + proxyPort + "/swagger/v1/swagger.json");

// #### API ####

// 🧠 In-Memory Plan-Speicher
var initialPlaene = new Dictionary<int, Dictionary<int, Stunde>>
{
    [23] = new()
    {
        [0] = new Stunde { Fach = "Mathe", Tag = "Montag", Zeit = "08:00" },
        [1] = new Stunde { Fach = "Informatik", Tag = "Dienstag", Zeit = "10:00" }
    }
};

var planManager = new PlanManager(initialPlaene);



app.MapGet("/plan/all", () =>
{
    var allePlaene = planManager
        .GetRaw()
        .ToDictionary(
            kw => kw.Key,
            kw => kw.Value.Select(p => new { Index = p.Key, Stunde = p.Value }).ToList()
        );

    return Results.Ok(allePlaene);
});

app.Run();

