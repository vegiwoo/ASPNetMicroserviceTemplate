// https://www.youtube.com/watch?v=rdWZo5PD9Ek&list=PLiaBqb-WDp-qX2cCT9dJLG3pTa3At-D7p&index=4
// https://www.youtube.com/watch?v=pj0hqTlxUX0


using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

       

        Task[] setUpTasks = [SetUpDbContext(builder), SetUpDI(builder)];
        for (int i = 0; i < setUpTasks.Length; i++)
        {
            Task.Run(() => setUpTasks[i]);
        }
        Task.WaitAll(setUpTasks);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add controllers
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

            // Add fake date
            PrepDB.PrepPopulation(app);
        }
   
        app.UseRouting();
        app.UseEndpoints(ep => 
        {
            ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder = ep.MapControllers();
        });


        //app.UseHttpsRedirection();

        app.Run();

        /// Sets up dependency injection
        static Task SetUpDI(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISomeModelsRepo, SomeModelRepo>();
            Trace.WriteLine("Repos with DI has been added to app.");
            return Task.CompletedTask;
        }

        /// Sets up DB context
        /// During development and testing, data can be stored in memory.
        static Task SetUpDbContext(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("InMem"));
            }
            else
            {
                // ... 
            }

            Trace.WriteLine("DbContext has been added to app.");
            return Task.CompletedTask;
        }
    }
}