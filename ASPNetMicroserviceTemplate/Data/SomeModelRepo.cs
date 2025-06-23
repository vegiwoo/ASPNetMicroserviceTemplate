using ASPNetMicroserviceTemplate.Model;

namespace ASPNetMicroserviceTemplate.Data 
{
    // Should be removed from the real project!
    public class SomeModelRepo(AppDBContext context) : ISomeModelsRepo
    {
        #region Fields
        private readonly AppDBContext context = context;

        #endregion

        #region Constructors
        // ... 
        #endregion

        #region Functionality
        public void CreateItem(SomeModel item)
        {
            if(item is null) 
                throw new ArgumentException("Item is null", nameof(item));

            context.SomeModels.Add(item);
        }

        public IEnumerable<SomeModel>? GetAllItems() => [.. context.SomeModels];

        public SomeModel? GetItemById(int id) => context.SomeModels.FirstOrDefault(u => Equals(u.Id, id));
        
        public bool SaveChanges() => context.SaveChanges() >= 0;
        #endregion
    }
}