using ASPNetMicroserviceTemplate.Model;

namespace ASPNetMicroserviceTemplate.Data
{
    public interface ISomeModelsRepo
    {
        #region Functionality
        bool SaveChanges();
        IEnumerable<SomeModel>? GetAllItems();
        SomeModel? GetItemById(int id);
        void CreateItem(SomeModel item);
        #endregion
    } 
}