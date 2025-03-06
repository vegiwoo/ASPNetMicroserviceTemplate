using ASPNetMicroserviceTemplate.Model;

namespace ASPNetMicroserviceTemplate.Data 
{
    public class SomeModelRepo : IRepo<SomeModel>
    {
        #region Fields
        private readonly AppDBContext context;
        #endregion

        #region Constructors
        public SomeModelRepo(AppDBContext context)
        {
            this.context = context;
        }
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
        
        public bool SaveShanges() => context.SaveChanges() >= 0;
        #endregion
    }
}