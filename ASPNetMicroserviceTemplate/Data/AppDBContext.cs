using ASPNetMicroserviceTemplate.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPNetMicroserviceTemplate.Data
{
    public class AppDBContext : DbContext
    {
        #region Properties
        /// <summary>
        /// Should be removed from the project!
        /// </summary>
        public DbSet<SomeModel> SomeModels { get; set; }
        #endregion

        #region Constructors
        public AppDBContext(DbContextOptions<AppDBContext> options) : base (options) {}
        #endregion
    }
}