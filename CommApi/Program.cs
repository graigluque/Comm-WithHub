using Microsoft.EntityFrameworkCore;
using CommApi.Data;
using CommApi.Hubs;
using CommApi;

using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Default Policy
builder.Services.AddCors();

// Add SignalR Service
builder.Services.AddSignalR().AddAzureSignalR();
/* Note: Not passing a ConnString to AddAzureSignalR() means 
it uses the default value with key Azure:SignalR:ConnectionString. */

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Using Azure CosmosDB
// builder.Services.AddDbContext<IDbContext, CosmosDbContext>();

// Using Azure SqlServerDB
builder.Services.AddDbContext<IDbContext, SqlServerDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ChatHub>();


var app = builder.Build();


// in general, before UserAuthorization
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // in general
    app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
}

app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<ChatHub>("/message");
    });

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
