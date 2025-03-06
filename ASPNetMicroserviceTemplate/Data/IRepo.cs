namespace ASPNetMicroserviceTemplate.Data 
{
    /// <summary>
    /// Basic interface of the entity repository.
    /// </summary>
    /// <typeparam name="T">Generalized entity type.</typeparam>
    public interface IRepo<T>
    {
        bool SaveShanges();
        IEnumerable<T>? GetAllItems();
        T? GetItemById(int id);
        void CreateItem(T item);
    }
} 