using Microsoft.AspNetCore.HttpOverrides;
using WeatherNow.Application.Dependency;
using WeatherNow.Infrastructure.Dependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices();

var frontendUrl = builder.Configuration["Frontend__Url"] ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins(frontendUrl, "http://localhost:80")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", ctx =>
    {
        ctx.Response.Redirect($"{ctx.Request.PathBase}/swagger");
        return Task.CompletedTask;
    });
}

app.UseCors("AllowReactApp");
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();