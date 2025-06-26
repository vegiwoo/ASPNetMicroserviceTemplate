using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Runtime-specific settings
        // if (builder.Environment.IsDevelopment())
        // {
        //     // Settings for dev environment
        // }
        // else
        // {
        //     // Settings for prod environment
        // }
        #endregion

        // Logging
        builder.Logging.ClearProviders().AddConsole();
        
        builder.Services
            .AddHttpClient()
            .AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<AppDBContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("InMem").UseInternalServiceProvider(sp);
            })
            .AddScoped<ISomeModelsRepo, SomeModelRepo>();
            //.AddHttpClient<IAnotherServiceDataClient, AnotherServiceDataClient>();
            //.AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add controllers
        builder.Services.AddMvc();
        builder.Services.AddControllers();

        // Add automapper 
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        var app = builder.Build();

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

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

        app.UseHttpsRedirection();

        // Add endpoint for liveness probes
        app.MapGet("/api/health", () =>
            Results.Ok($"{DateTime.Now}: Service successfully responds to requests"));

        // Add endpoint for readiness probes
        // - Для проверки добавляются все зависимости приложения, например, подключение к БД
        app.MapGet("/api/ready", (AppDBContext db) =>  
            db is not null && db.Database.CanConnect() ? 
            Results.Ok($"{DateTime.Now}: Service is ready to receive traffic") : 
            Results.Problem($"{DateTime.Now}: DB is not available."));

        app.Run();
    }
}