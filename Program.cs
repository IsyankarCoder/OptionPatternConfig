using Microsoft.Extensions.Options;
using OptionPatternConfig.ApiSettings;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiSettingsForOption>()
    .BindConfiguration(ApiSettingsForOption.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddHttpClient<IApiSettingsForOptionClient, ApiSettingsForOptionClient>();
builder.Services.AddScoped<IApiSettingsForOptionClient, ApiSettingsForOptionClient>();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().CacheOutput();
}

app.UseHttpsRedirection();

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
});
//.WithName("GetWeatherForecast");


app.MapGet("/data", static async (IApiSettingsForOptionClient apiClient, CancellationToken cancellationToken) =>
{
    var result = await apiClient.Execute(cancellationToken);
    return Results.Ok(result);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
