using ASPNetMicroserviceTemplate.Dtos;


/// Should be removed from the real project!
namespace ASPNetMicroserviceTemplate.SyncDataServices.Http
{
    public interface IAnotherServiceDataClient
    {
        public static string AnotherServiceSectionName { get; } = "AnotherService";
        public static string AnotherServiceBaseUrlName { get; } = "BaseUrl";
        public static string AnotherServicePostPathName { get; } = "PostPath";
        public Task SendPostFromThisServiceToAnotherService(SomeModelReadDto readDto);
    }
}