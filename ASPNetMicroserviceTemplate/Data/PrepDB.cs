using System.Diagnostics;
using ASPNetMicroserviceTemplate.Model;

namespace ASPNetMicroserviceTemplate.Data
{
    public static class PrepDB
    {
        #region Functionality
        public static void PrepPopulation(WebApplication app) 
        {
            using var serviceScope = app.Services.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>());
        }
        
        private static void SeedData(AppDBContext? context) 
        {
            if(context is null)
                throw new ArgumentException("DBContext for service not found", nameof(context));
            
            if(!context.SomeModels.Any()) 
            {
                Trace.WriteLine("Seeding data...");

                /// Should be removed from the real project!
                context.SomeModels.AddRange(
                   new SomeModel() { SomeStringName = "Some string 01", UpdateAt = DateTime.Now },
                   new SomeModel() { SomeStringName = "Some string 02", UpdateAt = DateTime.Now },  
                   new SomeModel() { SomeStringName = "Some string 03", UpdateAt = DateTime.Now }      
                );

                context.SaveChanges();
            }
            else 
            {
                Trace.WriteLine("Application has data");
            }
        }
        #endregion
    }
}