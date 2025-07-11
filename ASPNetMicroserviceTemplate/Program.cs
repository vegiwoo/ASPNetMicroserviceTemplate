using System.Diagnostics;
using System.Net.Http.Headers;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.HttpClients;
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

        builder.Services.AddHttpClient<RobohashClient>(client =>
        {
            client.BaseAddress = new Uri("http://robohash-cip"); // DNS-имя сервиса
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));
        });


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add controllers
        builder.Services.AddMvc();
        builder.Services.AddControllers();

        // Add automapper 
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = builder.Environment.IsDevelopment() ?
                StatusCodes.Status307TemporaryRedirect :
                StatusCodes.Status308PermanentRedirect;

            options.HttpsPort = builder.Environment.IsDevelopment() ? 5001 : 443;
        });

        // Add HSTS (HTTP Strict Transport Security)
        // builder.Services.AddHsts(options =>
        // {
        //     options.Preload = true;
        //     options.IncludeSubDomains = true;
        //     options.MaxAge = TimeSpan.FromDays(60);
        //     options.ExcludedHosts.Add("repack.pw");
        //     options.ExcludedHosts.Add("www.repack.pw");
        // });

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
        // else
        // { 
        //     app.UseHsts();
        // }

        app.UseRouting();
        app.UseEndpoints(ep => 
        {
            ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder = ep.MapControllers();
        });

        // Перенаправления HTTP-запросов на HTTPS
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