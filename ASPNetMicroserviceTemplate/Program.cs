using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<AppDBContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("InMem").UseInternalServiceProvider(sp);
            })
            .AddScoped<ISomeModelsRepo, SomeModelRepo>()
            .AddHttpClient<IAnotherServiceDataClient, AnotherServiceDataClient>();
            // .AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add controllers
        builder.Services.AddMvc();
        builder.Services.AddControllers();

        // Add automapper 
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Add console trace listener 
            Trace.Listeners.Add(new ConsoleTraceListener());

            // Add fake datа
            PrepDB.PrepPopulation(app);
        }

        app.UseRouting();
        app.UseEndpoints(ep => 
        {
            ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder = ep.MapControllers();
        });

        app.Run();
    }
}