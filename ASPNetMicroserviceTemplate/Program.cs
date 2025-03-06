// https://www.youtube.com/watch?v=rdWZo5PD9Ek&list=PLiaBqb-WDp-qX2cCT9dJLG3pTa3At-D7p&index=4

using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SetUpDI(ref builder);
AddDbContext(ref builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Add console trace listener 
    Trace.Listeners.Add(new ConsoleTraceListener());
}

app.UseHttpsRedirection();

PrepDB.PrepPopulation(app);

app.Run();

/// Sets up dependency injection
static void SetUpDI(ref WebApplicationBuilder builder) 
{
    builder.Services.AddScoped<IRepo<SomeModel>, SomeModelRepo>();
}

/// Sets up DB context
/// During development and testing, data can be stored in memory.
static void AddDbContext(ref WebApplicationBuilder builder) 
{
    if(builder.Environment.IsDevelopment())
    {
        builder.Services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("InMem"));
    } 
    else 
    {
        // ... 
    }
}