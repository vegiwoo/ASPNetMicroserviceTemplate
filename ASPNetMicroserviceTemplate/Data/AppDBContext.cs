using ASPNetMicroserviceTemplate.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNetMicroserviceTemplate.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        #region Properties
        /// <summary>
        /// Should be removed from the real project!
        /// </summary>
        public DbSet<SomeModel> SomeModels { get; set; }

        #endregion
    }
}